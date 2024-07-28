import { useNavigate, Link as RouterLink } from 'react-router-dom';
import { useEffect, useState } from 'react';
import openErpApi from '../../../services/OpenErpApi';
import { SnackbarProvider } from 'notistack';

import LoadingPage from '../../../utils/LoadingPage';

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
import { Button } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';

interface RowProps {
  id: string;
  name: string;
  stateName: string;
}

interface DetailsButtonProps {
  id: number;
}

export default function index() {
  const [loadingPage, setLoadingPage] = useState(true);
  const navigate = useNavigate();
  const [rows, setRows] = useState<RowProps[]>([]);

  useEffect(() => {
    openErpApi.get(`users/`).then(response => {
      setRows(response.data)
    }).finally(() => {
      setLoadingPage(false)
    });
  }, []);

  function CustomToolbar() {
    return (
      <GridToolbarContainer>
        <Button variant="text" size='small' component={RouterLink} to='create'>
          <AddIcon fontSize='small' sx={{ paddingRight: '5px' }} />
          New User
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

  const DetailsButton: React.FC<DetailsButtonProps> = ({ id }) => {
    const handleClick = () => {
      navigate(`${id}/edit`);
    };
  
    return (
      <Button variant="text" color="primary" onClick={handleClick} size='small'>
        Details
      </Button>
    );
  };

  const columns: GridColDef<(typeof rows)[number]>[] = [
    { field: 'id', headerName: 'ID', width: 90 },
    { field: 'inactiveDate', headerName: 'Inactive Date', minWidth: 150, flex: 1, },
    { field: 'employee', headerName: 'Employee', minWidth: 150, flex: 1, },
    { field: 'username', headerName: 'Username', minWidth: 150, flex: 1, },
    { field: 'roles', headerName: 'Groups', minWidth: 100, flex: 1, },
    { field: 'lastPasswordUpdatedAt', headerName: 'Last Password Updated At', minWidth: 50, flex: 1, },
    {
      field: 'details',
      headerName: 'Actions',
      minWidth: 120,
      flex: 1,
      renderCell: (params: GridRenderCellParams<any>) => <DetailsButton id={params.row.id} />,
    },
  ];

  return (
    <>
      {
        loadingPage
        ? <LoadingPage />
        : <>
          <Box sx={{ height: '100%', width: '100%' }}>
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