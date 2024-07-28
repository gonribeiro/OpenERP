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
import InputText from '../../../components/Form/InputText';
import SelectAutocomplete from '../../../components/Form/SelectAutocomplete';

import { Grid } from '@mui/material';

interface FormInputProps {
  name: string;
  currency: string;
  minSalary: string | null;
  maxSalary: string | null;
  zipCode: string | null;
  cityId: number | null;
  productAndServiceDescription: string | null;
}

const Details = () => {
  const [isLoading, setIsLoading] = useState(true);
  const { handleSubmit, control, reset, formState: { isSubmitting } } = useForm<FormInputProps>({
    defaultValues: {
      name: '',
      currency: 'USD',
      minSalary: null,
      maxSalary: null,
    }
  });
  const [currencyTypes, setCurrencyTypes] = useState([]);
  const location = useLocation();
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    const promises = [openErpApi.get(`enums/currency-types`)];

    if (location.pathname !== '/jobs/create')
      promises.push(openErpApi.get(`jobs/${id}`));

    Promise.all(promises)
      .then(([currencyTypes, job]) => {
        setCurrencyTypes(currencyTypes.data);

        if (location.pathname !== '/jobs/create') {
          job.data.maxSalary = formatToCurrency(job.data.maxSalary);
          job.data.minSalary = formatToCurrency(job.data.minSalary);

          reset(job.data);
        }
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, [location.pathname, id]);

  const onSubmit: SubmitHandler<FormInputProps> = async (data) => {
    data.maxSalary = removeCurrencyFormatting(data.maxSalary);
    data.minSalary = removeCurrencyFormatting(data.minSalary);

    if (location.pathname === '/jobs/create') {
      await openErpApi.post(`/jobs`, data)
        .then(response => {
          navigate(`/${response.data.redirectTo}`);
        });
    } else {
      await openErpApi.put(`jobs/${id}`, data);
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
              <PageTitle name={"Job"} />
            </Grid>
            <Grid item xs={6} md={6} container justifyContent="flex-end">
              <BackButton url={'/jobs'} />
            </Grid>
            <Grid item xs={12} md={12}>
              <InputText
                name="name"
                control={control}
                rules={{required: true, minLength: 3, maxLength: 120}}
              />
            </Grid>
            <Grid item xs={12} md={4}>
              <SelectAutocomplete
                name="currency"
                control={control}
                rules={{required: true}}
                options={currencyTypes}
              />
            </Grid>
            <Grid item xs={12} md={4}>
              <InputText
                name="minSalary"
                control={control}
                rules={{maxLength: 11}}
                label='Mininum Salary'
              />
            </Grid>
            <Grid item xs={12} md={4}>
              <InputText
                name="maxSalary"
                control={control}
                rules={{maxLength: 11}}
                label='Maximum Salary'
              />
            </Grid>
            <Grid item xs={6} md={6}>
              <SaveButton loading={isSubmitting} />
            </Grid>
            {
              location.pathname !== '/jobs/create'
              ? <Grid item xs={6} md={6} container justifyContent="flex-end">
                <ModalDelete
                  url={`jobs/${id}`}
                  title={'Job'}
                  text={"Are you sure you want to delete this job?\
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
};

export default Details;