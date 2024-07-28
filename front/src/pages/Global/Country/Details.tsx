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

import { Grid } from '@mui/material';

interface FormInputProps {
  name: string;
  nationality: string;
}

const Details = () => {
  const [isLoading, setIsLoading] = useState(true);
  const { handleSubmit, control, reset, formState: { isSubmitting } } = useForm<FormInputProps>({
    defaultValues: { name: '', nationality: '' }
  });
  const location = useLocation();
  const navigate = useNavigate();
  const {id} = useParams();

  useEffect(() => {
    if (location.pathname !== '/countries/create') {
      openErpApi.get(`countries/${id}`)
        .then(response => {
          reset(response.data);
        }).finally(() => {
          setIsLoading(false);
        });
    } else {
      setIsLoading(false);
    }
  }, []);

  const onSubmit: SubmitHandler<FormInputProps> = async (data) => {
    if (location.pathname === '/countries/create') {
      await openErpApi.post(`/countries`, data)
        .then(response => {
          navigate(`/${response.data.redirectTo}`)
        });
    } else {
      await openErpApi.put(`countries/${id}`, data);
    }
  };

  return (
    <>
      {
        isLoading
        ? <LoadingPage />
        : <form
          onSubmit={handleSubmit(onSubmit)}
          noValidate
          autoComplete="off"
        >
          <Grid container spacing={2}>
            <Grid item xs={6} md={6}>
              <PageTitle name={"Country"} />
            </Grid>
            <Grid item xs={6} md={6} container justifyContent="flex-end">
              <BackButton url={'/countries'} />
            </Grid>
            <Grid item xs={12} md={6}>
              <InputText
                name="name"
                control={control}
                rules={{required: true, minLength: 3, maxLength: 120}}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <InputText
                name="nationality"
                control={control}
                rules={{required: true, minLength: 3, maxLength: 120}}
              />
            </Grid>
            <Grid item xs={6} md={6}>
              <SaveButton loading={isSubmitting} />
            </Grid>
            {
              location.pathname !== '/countries/create'
              ? <Grid item xs={6} md={6} container justifyContent="flex-end">
                <ModalDelete
                  url={`countries/${id}`}
                  title={'Country'}
                  text={"Are you sure you want to delete this country? All related states\
                  and cities will be deleted, and the data cannot be restored."}
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