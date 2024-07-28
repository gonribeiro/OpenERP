import { TextField } from '@mui/material';
import { Controller, FieldValues, FieldPath } from 'react-hook-form';

interface ValidationRules {
    required?: boolean;
}

interface ControllerProps<TFieldValues extends FieldValues = FieldValues> {
    name: FieldPath<TFieldValues>;
    control: any;
    rules?: ValidationRules;
    label?: string;
}

const InputDate = <TFieldValues extends FieldValues = FieldValues>({
    name,
    control,
    rules,
    label,
}: ControllerProps<TFieldValues>) => {
    const dynamicRules = {
        required: !rules?.required ? false : "Required",
    };

    const modifiedLabel = `${label ?? name.charAt(0).toUpperCase() + name.slice(1)}${dynamicRules.required ? ' *' : ''}`;

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
                    type="date"
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

export default InputDate;