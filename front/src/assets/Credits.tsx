import { Grid, Link } from '@mui/material';
import Typography from '@mui/material/Typography';
import GitHubIcon from '@mui/icons-material/GitHub';

function Credits() {
  return (
    <Grid style={{ textAlign: "center" }}>
        <Typography variant="caption" display="block" gutterBottom>
            <Link
                underline="hover"
                href="https://github.com/gonribeiro"
                target="_blank"
                rel="noopener noreferrer"
            >
                <GitHubIcon style={{ fontSize: 15 }} /> OpenERP - Created by Tiago Ribeiro
            </Link>
        </Typography>
    </Grid>
  );
}

export default Credits;