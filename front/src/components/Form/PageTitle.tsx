import Typography from '@mui/material/Typography';
import { useLocation } from 'react-router-dom';

function PageTitle(props: { name: string }) {
  const location = useLocation();

  return (
    <Typography variant="h5" gutterBottom>
        { location.pathname.includes("create") ? 'Create' : 'Edit'} {props.name}
    </Typography>
  );
}

export default PageTitle;