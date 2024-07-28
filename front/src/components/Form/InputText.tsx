import { TextField } from '@mui/material';
import { Controller, FieldValues, FieldPath } from 'react-hook-form';

interface ValidationRules {
    required?: boolean;
    minLength?: number;
    maxLength?: number;
    currencyFormat?: boolean;
}

interface ControllerProps<TFieldValues extends FieldValues = FieldValues> {
    name: FieldPath<TFieldValues>;
    control: any;
    rules?: ValidationRules;
    label?: string;
    multiline?: number;
}

const InputText = <TFieldValues extends FieldValues = FieldValues>({
    name,
    control,
    rules,
    label,
    multiline,
}: ControllerProps<TFieldValues>) => {
    const dynamicRules = {
        required: rules?.required ? "Required" : undefined,
        minLength: rules?.minLength ? { value: rules.minLength, message: `Must be at least ${rules.minLength} characters long` } : undefined,
        maxLength: rules?.maxLength ? { value: rules.maxLength, message: `Must be at most ${rules.maxLength} characters long` } : undefined,
        pattern: rules?.currencyFormat ? { value: /^[0-9.,]*$/, message: "Must be in currency format (only numbers, points, and commas are allowed)" } : undefined,
    };

    const modifiedLabel = `${label ?? name.charAt(0).toUpperCase() + name.slice(1)}${rules?.required ? ' *' : ''}`;

    return (
        <Controller
            name={name}
            control={control}
            rules={dynamicRules}
            render={({ field: { value, onChange, onBlur }, fieldState: { error } }) => (
                <TextField
                    fullWidth
                    label={modifiedLabel}
                    variant="outlined"
                    value={value}
                    onChange={onChange}
                    onBlur={onBlur}
                    multiline={!!multiline}
                    rows={multiline}
                    error={!!error}
                    helperText={error ? error.message : ''}
                    InputLabelProps={{ shrink: true }}
                />
            )}
        />
    );
};

export default InputText;