export const formatToCurrency = (value: number | null): string | null => {
    return value !== null && value !== undefined
        ? value.toLocaleString('en-US', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        })
        : null;
};

export const removeCurrencyFormatting = (value: string | null): string | null => {
    return value ? value.replace(/,/g, '') : null;
};