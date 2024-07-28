import { ChangeEvent, useState } from 'react';

import openErpApi from '../../services/OpenErpApi';
import avatarNull from '../../assets/Images/avatarNull.jpg';

import { Box, Button, CircularProgress, Grid, ImageListItem, ImageListItemBar, Modal } from '@mui/material';

interface AvatarProps {
    employeeId: number;
    initialAvatarId?: string | null;
    initialAvatar?: string | null;
}

function Avatar({ employeeId, initialAvatarId, initialAvatar }: AvatarProps) {
    const [openModalAvatarUpload, setOpenModalAvatarUpload] = useState(false);
    const [file, setFile] = useState<File | null>(null);
    const [avatar, setAvatar] = useState(initialAvatar);
    const [avatarId, setAvatarId] = useState(initialAvatarId);
    const [submitProgress, setSubmitProgress] = useState(false);

    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files.length > 0) {
            setFile(event.target.files[0]);
        }
    };

    const handleUpload = () => {
        if (!file) return;

        setSubmitProgress(true);

        const formData = new FormData();
        formData.append('file', file);

        if (!avatarId) {
            formData.append('Description', 'Employee Photo');
            formData.append('modelType', 'Employee');
            formData.append('modelId', employeeId.toString());

            openErpApi.post('fileStorages', formData)
                .then((response) => {
                    setAvatarId(response.data.fileStorage.id)
                    setAvatar(URL.createObjectURL(file))
                })
                .finally(() => {
                    setSubmitProgress(false);
                    setOpenModalAvatarUpload(false);
                    setFile(null);
                });
        } else {
            openErpApi.put(`fileStorages/${avatarId}`, formData)
                .then(() => {
                    setAvatar(URL.createObjectURL(file))
                })
                .finally(() => {
                    setSubmitProgress(false);
                    setOpenModalAvatarUpload(false);
                    setFile(null);
                });
        }
    };

    return (
        <>
            <ImageListItem
                sx={{ borderRadius: '50%', overflow: 'hidden', cursor: 'pointer' }}
                onClick={() => setOpenModalAvatarUpload(true)}
            >
                <img
                    src={avatar ?? avatarNull}
                    loading="lazy"
                    style={{ width: 200, height: 200, objectFit: 'cover', borderRadius: '50%' }}
                />
                <ImageListItemBar
                    subtitle="Upload"
                    sx={{
                        '& .MuiImageListItemBar-subtitle': {
                            textAlign: 'center',
                            width: '100%',
                            position: 'center',
                        },
                    }}
                />
            </ImageListItem>
            <Modal
                open={openModalAvatarUpload}
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
                    {file && (
                        <Grid item xs={12} md={12} container justifyContent="center">
                            <img
                                src={URL.createObjectURL(file)}
                                loading="lazy"
                                style={{ width: 200, height: 200, objectFit: 'cover', borderRadius: '50%' }}
                            />
                        </Grid>
                    )}
                    <Box sx={{ textAlign: 'center', mt: 2, mb: 6 }}>
                        <input
                            accept="image/*"
                            style={{ display: 'none' }}
                            id="raised-button-file"
                            type="file"
                            onChange={handleFileChange}
                        />
                        <label htmlFor="raised-button-file">
                            <Button variant="contained" component="span">
                                Select Photo
                            </Button>
                        </label>
                    </Box>
                    <Grid container justifyContent="space-between" sx={{ mt: 2 }}>
                        { submitProgress
                            ? <CircularProgress />
                            : <>
                                <Grid item>
                                    <Button
                                        variant="contained"
                                        onClick={handleUpload}
                                        disabled={!file}
                                    >
                                        Upload
                                    </Button>
                                </Grid>
                                <Grid item>
                                    <Button
                                        variant="contained"
                                        color="error"
                                        onClick={() => {
                                            setOpenModalAvatarUpload(false);
                                            setFile(null);
                                        }}
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
};

export default Avatar;