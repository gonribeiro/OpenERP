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
import SelectAutocomplete from '../../../components/Form/SelectAutocomplete';
import InputDate from '../../../components/Form/InputDate';

import { Grid } from '@mui/material';

interface FormInputProps {
    employeeId: string;
    jobId: string;
    departmentId: string;
    startDate: string;
    endDate: string | null;
}

const Details = () => {
    const [isLoading, setIsLoading] = useState(true);
    const [jobs, setJobs] = useState([]);
    const [departments, setDepartments] = useState([]);
    const location = useLocation();
    const navigate = useNavigate();
    const { id, jobHistoryId } = useParams();

    const { handleSubmit, control, reset, formState: { isSubmitting } } = useForm<FormInputProps>({
        defaultValues: {
            employeeId: id,
            jobId: '',
            departmentId: '',
            startDate: '',
            endDate: null,
        }
    });

    useEffect(() => {
        const promises = [
            openErpApi.get(`jobs/`),
            openErpApi.get(`departments/`),
        ];

        if (!location.pathname.includes('jobHistories/create'))
            promises.push(openErpApi.get(`jobHistories/${jobHistoryId}`));

        Promise.all(promises)
          .then(([jobs, departments, jobHistories]) => {
                setJobs(jobs.data);
                setDepartments(departments.data);

                if (!location.pathname.includes('jobHistories/create'))
                    reset(jobHistories.data);
          })
          .finally(() => {
                setIsLoading(false);
          });
    }, [location.pathname, id, jobHistoryId]);

    const onSubmit: SubmitHandler<FormInputProps> = async (data) => {
        if (data.endDate === '')
            data.endDate = null

        if (location.pathname.includes('jobHistories/create')) {
            await openErpApi.post(`/jobHistories`, data)
                .then(response => {
                    navigate(`/${response.data.redirectTo}`);
                });
        } else {
            await openErpApi.put(`jobHistories/${jobHistoryId}`, data);
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
                            <PageTitle name={"Job History"} />
                        </Grid>
                        <Grid item xs={6} md={6} container justifyContent="flex-end">
                            <BackButton url={`/employees/${id}/edit`} name='Employee' />
                            <BackButton url={`/employees/${id}/jobHistories`} name='Job Histories'/>
                        </Grid>
                        <Grid item xs={12} md={3}>
                            <SelectAutocomplete
                                name={`jobId`}
                                control={control}
                                rules={{required: true}}
                                options={jobs}
                                label="Job"
                            />
                        </Grid>
                        <Grid item xs={12} md={3}>
                            <SelectAutocomplete
                                name={`departmentId`}
                                control={control}
                                rules={{required: true}}
                                options={departments}
                                label="Department"
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
                                label='End Date'
                            />
                        </Grid>
                        <Grid item xs={6} md={6}>
                            <SaveButton loading={isSubmitting} />
                        </Grid>
                        {
                            !location.pathname.includes('jobHistories/create')
                            ? <Grid item xs={6} md={6} container justifyContent="flex-end">
                                <ModalDelete
                                    url={`jobHistories/${jobHistoryId}`}
                                    title={'Job History'}
                                    text={"Are you sure you want to delete this job history?\
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