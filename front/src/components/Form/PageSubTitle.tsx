import Typography from '@mui/material/Typography';

function PageSubTitle(props: { name: string }) {
  return (
    <Typography variant="h6" gutterBottom>
        {props.name}
    </Typography>
  );
}

export default PageSubTitle;