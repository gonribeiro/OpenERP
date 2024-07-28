import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import openErpApi from '../../services/OpenErpApi';

import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Modal from '@mui/material/Modal';
import { Button, CircularProgress, Grid } from '@mui/material';

interface ModalProps {
    url: string;
    title: string;
    text: string
}

export default function ModalDelete({ url, title, text }: ModalProps) {
    const navigate = useNavigate();
    const [submitProgress, setSubmitProgress] = useState(false);
    const [openModalDelete, setOpenModalDelete] = useState(false);

    const handleDelete = (e: React.FormEvent) => {
        e.preventDefault();
        setSubmitProgress(true)

        openErpApi.delete(`${url}`)
            .then(response => {
                setTimeout(() => {
                    navigate(`/${response.data.redirectTo}`);
                }, 2000)
            });
    };

    return (
        <>
            <Button
                variant="outlined"
                color="error"
                onClick={() => setOpenModalDelete(!openModalDelete)}
            >
                Delete
            </Button>
            <Modal
                open={openModalDelete}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={{
                    position: 'absolute' as 'absolute',
                    top: '50%',
                    left: '50%',
                    transform: 'translate(-50%, -50%)',
                    bgcolor: 'background.paper',
                    boxShadow: 24,
                    maxWidth: "400px",
                    minWidth: "324px",
                    padding: "22px",
                }}>
                    <Typography id="modal-modal-title" variant="h6" component="h2">
                        Delete {title}
                    </Typography>
                    <Typography id="modal-modal-description" sx={{ mt: 2 }}>
                        {text}
                    </Typography>
                    <Grid container justifyContent="space-between" sx={{ mt: 2 }}>
                        { submitProgress
                            ? <CircularProgress />
                            : <>
                                <Grid item>
                                    <Button
                                        variant="contained"
                                        onClick={handleDelete}
                                    >
                                        Confirm Delete
                                    </Button>
                                </Grid>
                                <Grid item>
                                    <Button
                                        variant="contained"
                                        color={"error"}
                                        onClick={() => setOpenModalDelete(false)}
                                    >
                                        Cancel
                                    </Button>
                                </Grid>
                            </>
                        }
                    </Grid>
                </Box>
            </Modal>
        </>
    );
}