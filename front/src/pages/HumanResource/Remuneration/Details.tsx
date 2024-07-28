import { useEffect, useState } from 'react';
import { useForm, SubmitHandler } from 'react-hook-form';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';

import openErpApi from '../../../services/OpenErpApi';
import LoadingPage from '../../../utils/LoadingPage';
import { formatToCurrency, removeCurrencyFormatting } from '../../../utils/formatCurrency';

import SaveButton from '../../../components/Form/SaveButton';
import PageTitle from '../../../components/Form/PageTitle';
import BackButton from '../../../components/Form/BackButton';
import ModalDelete from '../../../components/Form/ModalDelete';
import SelectAutocomplete from '../../../components/Form/SelectAutocomplete';
import InputDate from '../../../components/Form/InputDate';
import InputText from '../../../components/Form/InputText';

import { Grid } from '@mui/material';

interface FormInputProps {
    employeeId: string;
    currency: string;
    baseSalary: string;
    bonus: string | null;
    commission: string | null;
    otherAllowances: string | null;
    otherAllowancesDescription: string | null;
    startDate: string;
    endDate: string | null;
}

const Details = () => {
    const [isLoading, setIsLoading] = useState(true);
    const [currencyTypes, setCurrencyTypes] = useState([]);
    const location = useLocation();
    const navigate = useNavigate();
    const { id, remunerationId } = useParams();

    const { handleSubmit, control, reset, formState: { isSubmitting } } = useForm<FormInputProps>({
        defaultValues: {
            employeeId: id,
            currency: '',
            baseSalary: '',
            bonus: null,
            commission: null,
            otherAllowances: null,
            otherAllowancesDescription: null,
            startDate: '',
            endDate: null,
        }
    });

    useEffect(() => {
        const promises = [
            openErpApi.get(`enums/currency-types`),
        ];

        if (!location.pathname.includes('remunerations/create'))
            promises.push(openErpApi.get(`remunerations/${remunerationId}`));

        Promise.all(promises)
          .then(([currencyTypes, remuneration]) => {
                setCurrencyTypes(currencyTypes.data);

                if (!location.pathname.includes('remunerations/create')) {
                    remuneration.data.baseSalary = formatToCurrency(remuneration.data.baseSalary);
                    remuneration.data.bonus = formatToCurrency(remuneration.data.bonus);
                    remuneration.data.commission = formatToCurrency(remuneration.data.commission);
                    remuneration.data.otherAllowances = formatToCurrency(remuneration.data.otherAllowances);

                    reset(remuneration.data);
                }
          })
          .finally(() => {
                setIsLoading(false);
          });
    }, [location.pathname, id, remunerationId]);

    const onSubmit: SubmitHandler<FormInputProps> = async (data) => {
        data.endDate = data.endDate !== '' ? data.endDate : null;
        data.baseSalary = removeCurrencyFormatting(data.baseSalary);
        data.bonus = removeCurrencyFormatting(data.bonus);
        data.commission = removeCurrencyFormatting(data.commission);
        data.otherAllowances = removeCurrencyFormatting(data.otherAllowances);

        if (location.pathname.includes('remunerations/create')) {
            await openErpApi.post(`/remunerations`, data)
                .then(response => {
                    navigate(`/${response.data.redirectTo}`);
                });
        } else {
            await openErpApi.put(`remunerations/${remunerationId}`, data);
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
                            <PageTitle name={"Remuneration"} />
                        </Grid>
                        <Grid item xs={6} md={6} container justifyContent="flex-end">
                            <BackButton url={`/employees/${id}/edit`} name='Employee' />
                            <BackButton url={`/employees/${id}/remunerations`} name='Remunerations'/>
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
                                label='End Date'
                            />
                        </Grid>
                        <Grid item xs={12} md={3}>
                            <SelectAutocomplete
                                name="currency"
                                control={control}
                                rules={{required: true}}
                                options={currencyTypes}
                            />
                        </Grid>
                        <Grid item xs={12} md={3}>
                            <InputText
                                name="baseSalary"
                                control={control}
                                rules={{required: true, maxLength: 10, currencyFormat: true}}
                                label='Base Salary'
                            />
                        </Grid>
                        <Grid item xs={12} md={4}>
                            <InputText
                                name="bonus"
                                control={control}
                                rules={{maxLength: 10, currencyFormat: true}}
                            />
                        </Grid>
                        <Grid item xs={12} md={4}>
                            <InputText
                                name="commission"
                                control={control}
                                rules={{maxLength: 10, currencyFormat: true}}
                            />
                        </Grid>
                        <Grid item xs={12} md={4}>
                            <InputText
                                name="otherAllowances"
                                control={control}
                                rules={{maxLength: 10, currencyFormat: true}}
                                label='Other Allowances'
                            />
                        </Grid>
                        <Grid item xs={12} md={12}>
                            <InputText
                                name="otherAllowancesDescription"
                                control={control}
                                rules={{maxLength: 10}}
                                multiline={3}
                                label='Other Allowances Description'
                            />
                        </Grid>
                        <Grid item xs={6} md={6}>
                            <SaveButton loading={isSubmitting} />
                        </Grid>
                        {
                            !location.pathname.includes('remunerations/create')
                            ? <Grid item xs={6} md={6} container justifyContent="flex-end">
                                <ModalDelete
                                    url={`remunerations/${remunerationId}`}
                                    title={'Remuneration'}
                                    text={"Are you sure you want to delete this remuneration?\
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