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

interface EmployeeProps {
  id: string;
  name: string;
}

interface DepartmentInputProps {
  name: string;
  managerId: number | null;
}

const Details = () => {
  const [isLoading, setIsLoading] = useState(true);
  const { handleSubmit, control, reset, formState: { isSubmitting } } = useForm<DepartmentInputProps>({
    defaultValues: {
      name: '',
      managerId: null
    }
  });
  const [employees, setEmployees] = useState<EmployeeProps[]>([]);
  const location = useLocation();
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    const promises = [openErpApi.get(`employees/`)];

    if (location.pathname !== '/departments/create')
      promises.push(openErpApi.get(`departments/${id}`));

    Promise.all(promises)
      .then(([employees, department]) => {
        setEmployees(employees.data);

        if (location.pathname !== '/departments/create')
          reset(department.data);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, [location.pathname, id]);

  const onSubmit: SubmitHandler<DepartmentInputProps> = async (data) => {
    if (location.pathname === '/departments/create') {
      await openErpApi.post(`/departments`, data)
        .then(response => {
          navigate(`/${response.data.redirectTo}`);
        });
    } else {
      await openErpApi.put(`departments/${id}`, data);
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
              <PageTitle name={"Department"} />
            </Grid>
            <Grid item xs={6} md={6} container justifyContent="flex-end">
              <BackButton url={'/departments'} />
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
                name="managerId"
                control={control}
                options={employees}
                label="Manager"
              />
            </Grid>
            <Grid item xs={6} md={6}>
              <SaveButton loading={isSubmitting} />
            </Grid>
            {
              location.pathname !== '/departments/create'
              ? <Grid item xs={6} md={6} container justifyContent="flex-end">
                <ModalDelete
                  url={`departments/${id}`}
                  title={'Department'}
                  text={"Are you sure you want to delete this department?\
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