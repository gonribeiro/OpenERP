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

import { Grid } from '@mui/material';

interface CountryProps {
  id: string;
  name: string;
}

interface FormInputProps {
  name: string;
  countryId: string;
}

const Details = () => {
  const [isLoading, setIsLoading] = useState(true);
  const { handleSubmit, control, reset, formState: { isSubmitting } } = useForm<FormInputProps>({
    defaultValues: {
      name: '',
      countryId: ''
    }
  });
  const [countries, setCountries] = useState<CountryProps[]>([]);
  const location = useLocation();
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    const promises = [openErpApi.get(`countries/`)];

    if (location.pathname !== '/states/create')
      promises.push(openErpApi.get(`states/${id}`));

    Promise.all(promises)
      .then(([countries, state]) => {
        setCountries(countries.data);

        if (location.pathname !== '/states/create')
          reset(state.data);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, [location.pathname, id]);

  const onSubmit: SubmitHandler<FormInputProps> = async (data) => {
    if (location.pathname === '/states/create') {
      await openErpApi.post(`/states`, data)
        .then(response => {
          navigate(`/${response.data.redirectTo}`);
        });
    } else {
      await openErpApi.put(`states/${id}`, data);
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
              <PageTitle name={"State"} />
            </Grid>
            <Grid item xs={6} md={6} container justifyContent="flex-end">
              <BackButton url={'/states'} />
            </Grid>
            <Grid item xs={12} md={6}>
              <InputText
                name="name"
                control={control}
                rules={{required: true, minLength: 3, maxLength: 120}}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <SelectAutocomplete
                name="countryId"
                control={control}
                rules={{ required: true }}
                options={countries}
                label="Country"
              />
            </Grid>
            <Grid item xs={6} md={6}>
              <SaveButton loading={isSubmitting} />
            </Grid>
            {
              location.pathname !== '/states/create'
              ? <Grid item xs={6} md={6} container justifyContent="flex-end">
                <ModalDelete
                  url={`states/${id}`}
                  title={'State'}
                  text={"Are you sure you want to delete this state? All related\
                  cities will be deleted, and the data cannot be restored."}
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