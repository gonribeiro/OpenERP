import { Box, Chip, FormControl, FormHelperText, InputLabel, MenuItem, OutlinedInput, Select } from '@mui/material';
import { Controller, FieldPath, FieldValues } from 'react-hook-form';

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
    errors?: any;
    defaultValue?: string[];
}

const MultipleSelectChip = ({
    name,
    control,
    rules,
    label,
    options,
    errors,
    defaultValue = [],
}: SelectAutocompleteProps) => {
    const dynamicRules = {
        required: !rules?.required ? false : "Required",
    };

    const modifiedLabel = `${label ?? name.charAt(0).toUpperCase() + name.slice(1)}${dynamicRules.required ? ' *' : ''}`;

    return (
        <FormControl fullWidth error={!!errors[name]}>
            <InputLabel shrink={true}>{modifiedLabel}</InputLabel>
            <Controller
                name={name}
                control={control}
                defaultValue={defaultValue}
                rules={dynamicRules}
                render={({ field: { onChange, value } }) => (
                    <Select
                        multiple
                        value={value || []}
                        onChange={(event) => onChange(event.target.value as string[])}
                        input={<OutlinedInput label={modifiedLabel} />}
                        renderValue={(selected) => (
                            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5 }}>
                                {(selected as string[]).map((id) => {
                                    const option = options.find((opt) => opt.id === id);
                                    return (
                                        <Chip key={id} label={option ? option.name : ''} />
                                    );
                                })}
                            </Box>
                        )}
                        MenuProps={{
                            PaperProps: {
                                style: {
                                    maxHeight: 48 * 4.5 + 8,
                                    width: 250,
                                },
                            },
                        }}
                    >
                        {options.map((option) => (
                            <MenuItem key={option.id} value={option.id}>
                                {option.name}
                            </MenuItem>
                        ))}
                    </Select>
                )}
            />
            <FormHelperText>{errors[name]?.message}</FormHelperText>
        </FormControl>
    );
};

export default MultipleSelectChip;