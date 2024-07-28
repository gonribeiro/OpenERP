import { useEffect, useState } from 'react';
import { useForm, SubmitHandler, Controller } from 'react-hook-form';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';

import openErpApi from '../../../services/OpenErpApi';
import LoadingPage from '../../../utils/LoadingPage';

import SaveButton from '../../../components/Form/SaveButton';
import PageTitle from '../../../components/Form/PageTitle';
import BackButton from '../../../components/Form/BackButton';
import InputText from '../../../components/Form/InputText';

import { Alert, FormControlLabel, Grid, Switch } from '@mui/material';

interface FormInputProps {
  name: string;
  inactiveDate: Date | null;
}

const Details = () => {
  const [isLoading, setIsLoading] = useState(true);
  const { handleSubmit, control, reset, formState: { isSubmitting } } = useForm<FormInputProps>({
    defaultValues: {
      name: '',
      inactiveDate: null,
    }
  });
  const [roleIsInactive, setRoleIsInactive] = useState<any>(null);
  const location = useLocation();
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    if (location.pathname !== '/roles/create') {
      openErpApi.get(`roles/${id}`)
        .then(response => {
          reset(response.data);
          setRoleIsInactive(response.data.inactiveDate)
        })
        .finally(() => {
          setIsLoading(false);
        });
    } else {
      setIsLoading(false);
    }
  }, [location.pathname, id]);

  const onSubmit: SubmitHandler<FormInputProps> = async (data) => {
    if (location.pathname === '/roles/create') {
      await openErpApi.post(`/roles`, data)
        .then(response => {
          navigate(`/${response.data.redirectTo}`);
        });
    } else {
      await openErpApi.put(`roles/${id}`, data)
      .then(() => {
        setRoleIsInactive(data.inactiveDate);
      });
    }
  };

  return (
    <>
      {
        isLoading
          ? <LoadingPage />
          : <form onSubmit={handleSubmit(onSubmit)}>
          <Grid container spacing={2}>
            { roleIsInactive
              ? <Grid item xs={12} md={12}>
                <Alert severity="info">{`Role inactive since ${roleIsInactive}`}</Alert>
              </Grid>
              : <></>
            }
            <Grid item xs={6} md={6}>
              <PageTitle name={"Role"} />
            </Grid>
            <Grid item xs={6} md={6} container justifyContent="flex-end">
              <BackButton url={'/roles'} />
            </Grid>
            {
              location.pathname !== '/roles/create'
              ? <Grid item xs={12} md={12} container justifyContent="flex-end">
                <Controller
                  name="inactiveDate"
                  control={control}
                  render={({ field }) => (
                    <FormControlLabel
                      control={
                        <Switch
                          checked={!!field.value}
                          onChange={(event) => field.onChange(event.target.checked
                            ? new Date().toLocaleDateString('en-CA', { year: 'numeric', month: '2-digit', day: '2-digit' })
                            : null
                          )}
                        />
                      }
                      label={'Inactive'}
                    />
                  )}
                />
              </Grid>
              : <></>
            }
            <Grid item xs={12} md={12}>
              <InputText
                name="name"
                control={control}
                rules={{required: true, minLength: 3, maxLength: 120}}
              />
            </Grid>
            <Grid item xs={6} md={6}>
              <SaveButton loading={isSubmitting} />
            </Grid>
          </Grid>
          <SnackbarProvider/>
        </form>
      }
    </>
  );
};

export default Details;