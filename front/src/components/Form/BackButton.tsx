import { useNavigate } from 'react-router-dom';

import Button from '@mui/material/Button';
import ArrowBackIosIcon from '@mui/icons-material/ArrowBackIos';

interface BackButtonProps {
  url: string;
  name?: string;
}

function BackButton({ url, name }: BackButtonProps) {
  const navigate = useNavigate();

  const handleGoBack = () => {
    navigate(`${url}`);
  };

  return (
    <Button variant="text" onClick={handleGoBack}>
      <ArrowBackIosIcon fontSize='small' />
      {name ?? 'Back'}
    </Button>
  );
};

export default BackButton;