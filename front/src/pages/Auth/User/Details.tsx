import { useEffect, useState } from 'react';
import { useForm, SubmitHandler, Controller } from 'react-hook-form';
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
import SelectAutocompleteMultiple from '../../../components/Form/SelectAutocompleteMultiple';
import PageSubTitle from '../../../components/Form/PageSubTitle';
import InputPassword from '../../../components/Form/InputPassword';

import { Alert, FormControlLabel, Grid, Switch } from '@mui/material';

interface FormInputProps {
  inactiveDate: Date | null;
  username: string;
  password: string | null;
  employeeId: number | null;
  employee: string;
}

const Details = () => {
  const [isLoading, setIsLoading] = useState(true);
  const { handleSubmit, control, reset, formState: { errors, isSubmitting }, getValues } = useForm<FormInputProps>({
    defaultValues: {
      inactiveDate: null,
      employeeId: null,
      employee: '',
      username: '',
      password: null,
    }
  });
  const [userIsInactive, setUserIsInactive] = useState<any>(null);
  const [employees, setEmployees] = useState([]);
  const [roles, setRoles] = useState([]);
  const location = useLocation();
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    const promises = [openErpApi.get(`roles/actives`)];

    if (location.pathname === '/users/create') {
      promises.push(openErpApi.get(`employees/withoutUserAccount`));
    } else {
      promises.push(openErpApi.get(`users/${id}`));
    }

    Promise.all(promises)
      .then(([
        roles,
        response,
      ]) => {
        setRoles(roles.data);

        if (location.pathname === '/users/create') {
          setEmployees(response.data);
        } else {
          setUserIsInactive(response.data.inactiveDate)
          reset(response.data);
        }
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, [location.pathname, id]);

  const onSubmit: SubmitHandler<FormInputProps> = async (data) => {
    if (location.pathname === '/users/create') {
      await openErpApi.post(`/users`, data)
        .then(response => {
          response.data.password = null;
          reset(response.data);
          navigate(`/${response.data.redirectTo}`);
        });
    } else {
      await openErpApi.put(`users/${id}`, data)
      .then(() => {
        setUserIsInactive(data.inactiveDate);
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
            { userIsInactive
              ? <Grid item xs={12} md={12}>
                <Alert severity="info">{`User inactive since ${userIsInactive}`}</Alert>
              </Grid>
              : <></>
            }
            <Grid item xs={6} md={6}>
              <PageTitle name={"User"} />
            </Grid>
            <Grid item xs={6} md={6} container justifyContent="flex-end">
              <BackButton url={'/users'} />
            </Grid>
            {
              location.pathname === '/users/create'
              ? <Grid item xs={12} md={12}>
                <SelectAutocomplete
                  name="employeeId"
                  control={control}
                  rules={{ required: true }}
                  options={employees}
                  label="Employee"
                />
              </Grid>
              : <>
                <Grid item xs={12} md={10}>
                  <PageSubTitle name={getValues('employee')} />
                </Grid>
                <Grid item xs={12} md={2} container justifyContent="flex-end">
                  {
                    location.pathname !== '/employees/create'
                    ? <Controller
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
                    : <></>
                  }
                </Grid>
              </>
            }
            <Grid item xs={12} md={6}>
              <InputText
                name="username"
                control={control}
                rules={{required: true, minLength: 3, maxLength: 120}}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              {
                location.pathname === '/users/create'
                ? <InputPassword
                  name="password"
                  control={control}
                  rules={{required: true}}
                />
                : <InputPassword
                  name="password"
                  control={control}
                />
              }
            </Grid>
            <Grid item xs={12} md={12}>
              <SelectAutocompleteMultiple
                name="roleIds"
                control={control}
                options={roles}
                errors={errors}
                label="Roles"
              />
            </Grid>
            <Grid item xs={6} md={6}>
              <SaveButton loading={isSubmitting} />
            </Grid>
            {
              location.pathname !== '/users/create'
              ? <Grid item xs={6} md={6} container justifyContent="flex-end">
                <ModalDelete
                  url={`users/${id}`}
                  title={'User'}
                  text={"Are you sure you want to delete this user?\
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