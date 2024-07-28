import { useForm, SubmitHandler } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';

import openErpApi from '../../../services/OpenErpApi';

import InputPassword from '../../../components/Form/InputPassword';
import SaveButton from '../../../components/Form/SaveButton';

import { Grid, Typography } from '@mui/material';

interface FormInputProps {
  password: string;
  confirmPassword: string;
}

const Details = () => {
    const { handleSubmit, control, formState: { isSubmitting }, getValues } = useForm<FormInputProps>({
        defaultValues: { password: '', confirmPassword: '' }
    });
    const navigate = useNavigate();

    const onSubmit: SubmitHandler<FormInputProps> = async (data) => {
        await openErpApi.put(`/users/updatePassword`, data)
            .then(response => {
                navigate(`/${response.data.redirectTo}`)
            });
    };

    return (
        <form
            onSubmit={handleSubmit(onSubmit)}
            noValidate
            autoComplete="off"
        >
            <Grid container spacing={2}>
                <Grid item xs={12} md={12}>
                    <Typography variant="h5" gutterBottom>
                        Update your password
                    </Typography>
                </Grid>
                <Grid item xs={12} md={12}>
                    <InputPassword
                        name="password"
                        control={control}
                        rules={{required: true}}
                    />
                </Grid>
                <Grid item xs={12} md={12}>
                    <InputPassword
                        name="confirmPassword"
                        control={control}
                        label="Confirm Password"
                        rules={{required: true, confirmPassword: getValues("password")}}
                    />
                </Grid>
                <Grid item xs={12} md={12}>
                    <SaveButton loading={isSubmitting} />
                </Grid>
            </Grid>
            <SnackbarProvider/>
        </form>
    );
};

export default Details;