import { useNavigate, Link as RouterLink, useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { SnackbarProvider } from 'notistack';

import openErpApi from '../../../services/OpenErpApi';
import LoadingPage from '../../../utils/LoadingPage';
import PageTitle from '../../../components/Form/PageTitle';
import BackButton from '../../../components/Form/BackButton';

import Box from '@mui/material/Box';
import {
  DataGrid,
  GridColDef,
  GridRenderCellParams,
  GridToolbarContainer,
  GridToolbarExport,
  GridToolbarFilterButton,
  GridToolbarQuickFilter
} from '@mui/x-data-grid';
import { Button, Grid } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';

interface DetailsButtonProps {
  jobHistoryId: number;
}

export default function index() {
  const [loadingPage, setLoadingPage] = useState(true);
  const navigate = useNavigate();
  const [rows, setRows] = useState([]);
  const { id } = useParams();

  useEffect(() => {
    openErpApi.get(`employees/${id}/jobHistories`).then(response => {
      setRows(response.data)
    }).finally(() => {
      setLoadingPage(false)
    });
  }, [location.pathname]);

  function CustomToolbar() {
    return (
      <GridToolbarContainer>
        <Button variant="text" size='small' component={RouterLink} to='create'>
          <AddIcon fontSize='small' sx={{ paddingRight: '5px' }} />
          New Job History
        </Button>
        <GridToolbarFilterButton />
        <GridToolbarExport />
        <Box sx={{ flexGrow: 1 }} />
        <GridToolbarQuickFilter
          slotProps={{
            toolbar: {
              showQuickFilter: true,
            },
          }}
        />
      </GridToolbarContainer>
    );
  }

  const DetailsButton: React.FC<DetailsButtonProps> = ({ jobHistoryId }) => {
    const handleClick = () => {
      navigate(`/employees/${id}/jobHistories/${jobHistoryId}/edit`);
    };

    return (
      <Button variant="text" color="primary" onClick={handleClick} size='small'>
        Details
      </Button>
    );
  };

  const columns: GridColDef<(typeof rows)[number]>[] = [
    { field: 'id', headerName: 'ID', width: 90 },
    { field: 'job', headerName: 'Job', minWidth: 200, flex: 1, },
    { field: 'department', headerName: 'Department', minWidth: 200, flex: 1, },
    { field: 'startDate', headerName: 'Start Date', minWidth: 150, flex: 1, },
    { field: 'endDate', headerName: 'End Date', minWidth: 150, flex: 1, },
    {
      field: 'details',
      headerName: 'Actions',
      minWidth: 120,
      flex: 1,
      renderCell: (params: GridRenderCellParams<any>) => <DetailsButton jobHistoryId={params.row.id} />,
    },
  ];

  return (
    <>
      {
        loadingPage
        ? <LoadingPage />
        : <>
          <Box sx={{ height: '100%', width: '100%' }}>
            <Grid container spacing={2}>
              <Grid item xs={6} md={6}>
                <PageTitle name={"Job History"} />
              </Grid>
              <Grid item xs={6} md={6} container justifyContent="flex-end">
                <BackButton url={`/employees/${id}/edit`} name='Employee' />
              </Grid>
            </Grid>
            <DataGrid
              rows={rows}
              columns={columns}
              initialState={{
                pagination: {
                  paginationModel: {
                    pageSize: window.innerWidth <= 600 ? 10 : 25,
                  },
                },
                density: 'compact',
                filter: {
                  filterModel: {
                    items: [],
                    quickFilterValues: [''],
                  },
                },
              }}
              pageSizeOptions={[10, 25, 50, 100, 250]}
              slots={{ toolbar: CustomToolbar }}
              disableRowSelectionOnClick
              disableDensitySelector
              autoHeight
            />
          </Box>
          <SnackbarProvider/>
        </>
      }
    </>
  );
}