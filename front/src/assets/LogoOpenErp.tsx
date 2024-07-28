import Typography from '@mui/material/Typography';

interface LogoOpenErpProps {
  xs?: string;
  md?: string;
  flexGrow?: number;
}

function LogoOpenERP({xs, md, flexGrow}: LogoOpenErpProps) {
  return (
    <Typography
      variant="h6"
      noWrap
      component="a"
      sx={{
        mr: 2,
        display: { xs: xs ?? 'none', md: md ?? 'flex' },
        flexGrow: flexGrow ?? 0,
        fontFamily: 'monospace',
        fontWeight: 700,
        letterSpacing: '.2rem',
        color: 'inherit',
        textDecoration: 'none',
      }}
    >
      OpenERP
    </Typography>
  );
}

export default LogoOpenERP;