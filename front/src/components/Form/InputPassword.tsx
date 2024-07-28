import { TextField } from '@mui/material';
import { Controller, FieldValues, FieldPath } from 'react-hook-form';

interface ValidationRules {
    required?: boolean;
    confirmPassword?: string;
}

interface ControllerProps<TFieldValues extends FieldValues = FieldValues> {
    name: FieldPath<TFieldValues>;
    control: any;
    label?: string;
    rules?: ValidationRules;
}

const InputPassword = <TFieldValues extends FieldValues = FieldValues>({
    name,
    control,
    label,
    rules,
}: ControllerProps<TFieldValues>) => {
    const dynamicRules = {
        required: rules?.required ? "Required" : undefined,
        pattern: {
            value: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.#])[A-Za-z\d@$!%*?&.#]{6,}$/,
            message: 'Password must contain at least one uppercase letter,\
            one lowercase letter, one number, and one special character (!?@$%&.#)',
        },
        validate: rules?.confirmPassword
            ? (value: string) => value === rules.confirmPassword || 'Passwords do not match'
            : undefined,
    };

    return (
        <Controller
            name={name}
            control={control}
            rules={{
                ...dynamicRules,
                minLength: { value: 6, message: 'Password must be at least 6 characters long' },
                maxLength: { value: 50, message: 'Password cannot exceed 50 characters' },
            }}
            render={({ field: { value, onChange, onBlur }, fieldState: { error } }) => (
                <TextField
                    type="password"
                    fullWidth
                    label={`${label ?? name.charAt(0).toUpperCase() + name.slice(1)}${rules?.required ? ' *' : ''}`}
                    variant="outlined"
                    value={value}
                    onChange={onChange}
                    onBlur={onBlur}
                    error={!!error}
                    helperText={error ? error.message : ''}
                    InputLabelProps={{ shrink: true }}
                />
            )}
        />
    );
};

export default InputPassword;