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

interface StateProps {
  id: string;
  name: string;
}

interface FormInputProps {
  name: string;
  stateId: string;
}

const Details = () => {
  const [isLoading, setIsLoading] = useState(true);
  const { handleSubmit, control, reset, formState: { isSubmitting } } = useForm<FormInputProps>({
    defaultValues: {
      name: '',
      stateId: ''
    }
  });
  const [states, setStates] = useState<StateProps[]>([]);
  const location = useLocation();
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    const promises = [openErpApi.get(`states/`)];

    if (location.pathname !== '/cities/create')
      promises.push(openErpApi.get(`cities/${id}`));

    Promise.all(promises)
      .then(([states, city]) => {
        setStates(states.data);

        if (location.pathname !== '/cities/create')
          reset(city.data);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, [location.pathname, id]);

  const onSubmit: SubmitHandler<FormInputProps> = async (data) => {
    if (location.pathname === '/cities/create') {
      await openErpApi.post(`/cities`, data)
        .then(response => {
          navigate(`/${response.data.redirectTo}`);
        });
    } else {
      await openErpApi.put(`cities/${id}`, data);
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
              <PageTitle name={"City"} />
            </Grid>
            <Grid item xs={6} md={6} container justifyContent="flex-end">
              <BackButton url={'/cities'} />
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
                name="stateId"
                control={control}
                rules={{ required: true }}
                options={states}
                label="State"
              />
            </Grid>
            <Grid item xs={6} md={6}>
              <SaveButton loading={isSubmitting} />
            </Grid>
            {
              location.pathname !== '/cities/create'
              ? <Grid item xs={6} md={6} container justifyContent="flex-end">
                <ModalDelete
                  url={`cities/${id}`}
                  title={'City'}
                  text={"Are you sure you want to delete this city?\
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