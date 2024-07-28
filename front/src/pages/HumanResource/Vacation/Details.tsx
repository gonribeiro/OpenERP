import { useEffect, useState } from 'react';
import { useForm, SubmitHandler } from 'react-hook-form';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';

import openErpApi from '../../../services/OpenErpApi';
import LoadingPage from '../../../utils/LoadingPage';

import SaveButton from '../../../components/Form/SaveButton';
import PageTitle from '../../../components/Form/PageTitle';
import BackButton from '../../../components/Form/BackButton';
import ModalDelete from '../../../components/Form/ModalDelete';
import InputText from '../../../components/Form/InputText';
import SelectAutocomplete from '../../../components/Form/SelectAutocomplete';
import InputDate from '../../../components/Form/InputDate';

import { Grid } from '@mui/material';

interface FormInputProps {
    employeeId: string;
    startDate: string;
    endDate: string;
    type: string;
    reason: string | null;
    approvedById: number | null;
}

const Details = () => {
    const [isLoading, setIsLoading] = useState(true);
    const [types, setTypes] = useState([]);
    const [employees, setEmployees] = useState([]);
    const location = useLocation();
    const navigate = useNavigate();
    const { id, vacationId } = useParams();

    const { handleSubmit, control, reset, formState: { isSubmitting } } = useForm<FormInputProps>({
        defaultValues: {
            employeeId: id,
            startDate: '',
            endDate: '',
            type: '',
            reason: '',
            approvedById: null,
        }
    });

    useEffect(() => {
        const promises = [
            openErpApi.get(`employees/`),
            openErpApi.get(`enums/vacation-types`),
        ];

        if (!location.pathname.includes('vacations/create'))
            promises.push(openErpApi.get(`vacations/${vacationId}`));

        Promise.all(promises)
          .then(([employees, types, vacations]) => {
                setEmployees(employees.data);
                setTypes(types.data);

                if (!location.pathname.includes('vacations/create'))
                    reset(vacations.data);
          })
          .finally(() => {
                setIsLoading(false);
          });
    }, [location.pathname, id, vacationId]);

    const onSubmit: SubmitHandler<FormInputProps> = async (data) => {
        if (location.pathname.includes('vacations/create')) {
            await openErpApi.post(`/vacations`, data)
                .then(response => {
                    navigate(`/${response.data.redirectTo}`);
                });
        } else {
            await openErpApi.put(`vacations/${vacationId}`, data);
        }
    };

    return (
        <>
            {
                isLoading
                    ? <LoadingPage />
                    : <form onSubmit={handleSubmit(onSubmit)}>
                    <Grid container spacing={2}>
                        <Grid item xs={6} md={6}>
                            <PageTitle name={"Vacation"} />
                        </Grid>
                        <Grid item xs={6} md={6} container justifyContent="flex-end">
                            <BackButton url={`/employees/${id}/edit`} name='Employee' />
                            <BackButton url={`/employees/${id}/vacations`} name='Vacations'/>
                        </Grid>
                        <Grid item xs={12} md={3}>
                            <SelectAutocomplete
                                name={`type`}
                                control={control}
                                rules={{ required: true }}
                                options={types}
                            />
                        </Grid>
                        <Grid item xs={12} md={3}>
                            <InputDate
                                name={`startDate`}
                                control={control}
                                rules={{required: true}}
                                label='Start Date'
                            />
                        </Grid>
                        <Grid item xs={12} md={3}>
                            <InputDate
                                name={`endDate`}
                                control={control}
                                rules={{required: true}}
                                label='End Date'
                            />
                        </Grid>
                        <Grid item xs={12} md={3}>
                            <SelectAutocomplete
                                name={`approvedById`}
                                control={control}
                                options={employees}
                                label="Approved By"
                            />
                        </Grid>
                        <Grid item xs={12} md={12}>
                            <InputText
                                name={`reason`}
                                control={control}
                                rules={{minLength: 3, maxLength: 255}}
                            />
                        </Grid>
                        <Grid item xs={6} md={6}>
                            <SaveButton loading={isSubmitting} />
                        </Grid>
                        {
                            !location.pathname.includes('vacations/create')
                            ? <Grid item xs={6} md={6} container justifyContent="flex-end">
                                <ModalDelete
                                    url={`vacations/${vacationId}`}
                                    title={'Vacation'}
                                    text={"Are you sure you want to delete this vacation?\
                                    The data cannot be restored."}
                                />
                            </Grid>
                            : <></>
                        }
                    </Grid>
                    <SnackbarProvider/>
                </form>
            }
        </>
    );
}

export default Details;