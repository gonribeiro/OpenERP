import { Controller, FieldValues, FieldPath } from 'react-hook-form';

import { FormControl, TextField } from '@mui/material';
import Autocomplete from '@mui/material/Autocomplete';

interface Options {
    id: string;
    name: string;
}

interface ValidationRules {
    required?: boolean;
}

interface SelectAutocompleteProps<TFieldValues extends FieldValues = FieldValues> {
    name: FieldPath<TFieldValues>;
    control: any;
    rules?: ValidationRules;
    label?: string;
    options: Options[];
}

const SelectAutocomplete = <TFieldValues extends FieldValues = FieldValues>({
    name,
    control,
    rules,
    label,
    options,
}: SelectAutocompleteProps<TFieldValues>) => {
    const dynamicRules = {
        required: !rules?.required ? false : "Required",
    };

    const modifiedLabel = `${label ?? name.charAt(0).toUpperCase() + name.slice(1)}${dynamicRules.required ? ' *' : ''}`;

    return (
        <FormControl fullWidth >
            <Controller
                name={name}
                control={control}
                rules={dynamicRules}
                render={({ field, fieldState: { error } }) => (
                    <Autocomplete
                        options={options}
                        getOptionLabel={(option) => option.name}
                        value={options.find(option => option.id === field.value) || null}
                        onChange={(event, newValue) => {
                            field.onChange(newValue ? newValue.id : null);
                        }}
                        renderInput={(params) => (
                            <TextField
                                {...params}
                                label={modifiedLabel}
                                variant="outlined"
                                error={!!error}
                                InputLabelProps={{ shrink: true }}
                                helperText={error ? error.message : ''}
                            />
                        )}
                    />
                )}
            />
        </FormControl>
    );
};

export default SelectAutocomplete;