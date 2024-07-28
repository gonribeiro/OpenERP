import Box from '@mui/material/Box';
import LinearProgress from '@mui/material/LinearProgress';

export default function LoadingPage() {
    return (
        <Box sx={{ width: '100%' }}>
            Loading
            <LinearProgress />
        </Box>
    );
}