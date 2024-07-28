import { useEffect, useState } from 'react';
import { useForm, SubmitHandler } from 'react-hook-form';
import { useLocation, useNavigate, useParams } from 'react-router-dom';

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
import { SnackbarProvider } from 'notistack';

interface FormInputProps {
    employeeId: string;
    institutionId: string;
    course: string | null;
    educationLevel: number | null;
    startDate: string;
    endDate: string;
}

const Details = () => {
    const [isLoading, setIsLoading] = useState(true);
    const [institutions, setInstitutions] = useState([]);
    const location = useLocation();
    const navigate = useNavigate();
    const { id, educationId } = useParams();

    const { handleSubmit, control, reset, formState: { isSubmitting } } = useForm<FormInputProps>({
        defaultValues: {
            employeeId: id,
            institutionId: '',
            course: '',
            educationLevel: null,
            startDate: '',
            endDate: '',
        }
    });

    useEffect(() => {
        const promises = [
            openErpApi.get(`companies/Education`), // Institutions
        ];

        if (!location.pathname.includes('educations/create'))
            promises.push(openErpApi.get(`educations/${educationId}`));

        Promise.all(promises)
          .then(([institutions, educations]) => {
                setInstitutions(institutions.data);

                if (!location.pathname.includes('educations/create'))
                    reset(educations.data);
          })
          .finally(() => {
                setIsLoading(false);
          });
    }, [location.pathname, id, educationId]);

    const onSubmit: SubmitHandler<FormInputProps> = async (data) => {
        if (location.pathname.includes('educations/create')) {
            await openErpApi.post(`/educations`, data)
                .then(response => {
                    navigate(`/${response.data.redirectTo}`);
                });
        } else {
            await openErpApi.put(`educations/${educationId}`, data);
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
                            <PageTitle name={"Education"} />
                        </Grid>
                        <Grid item xs={6} md={6} container justifyContent="flex-end">
                            <BackButton url={`/employees/${id}/edit`} name='Employee' />
                            <BackButton url={`/employees/${id}/educations`} name='Educations'/>
                        </Grid>
                        <Grid item xs={12} md={12}>
                            <SelectAutocomplete
                                name={`institutionId`}
                                control={control}
                                rules={{required: true}}
                                options={institutions}
                                label="Institution"
                            />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <InputText
                                name={`course`}
                                control={control}
                                rules={{required: true, minLength: 3, maxLength: 255}}
                            />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <InputText
                                name={`educationLevel`}
                                control={control}
                                rules={{required: true, minLength: 3, maxLength: 255}}
                                label='Education Level'
                            />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <InputDate
                                name={`startDate`}
                                control={control}
                                rules={{required: true}}
                                label='Start Date'
                            />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <InputDate
                                name={`endDate`}
                                control={control}
                                rules={{required: true}}
                                label='End Date or Expected Completion Date'
                            />
                        </Grid>
                        <Grid item xs={6} md={6}>
                            <SaveButton loading={isSubmitting} />
                        </Grid>
                        {
                            !location.pathname.includes('educations/create')
                            ? <Grid item xs={6} md={6} container justifyContent="flex-end">
                                <ModalDelete
                                    url={`educations/${educationId}`}
                                    title={'Education'}
                                    text={"Are you sure you want to delete this education?\
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