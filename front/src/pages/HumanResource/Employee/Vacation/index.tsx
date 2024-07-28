import { useEffect, useState } from 'react';
import { SnackbarProvider } from 'notistack';

import openErpApi from '../../../../services/OpenErpApi';
import LoadingPage from '../../../../utils/LoadingPage';

import { Grid, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material';
import LuggageIcon from '@mui/icons-material/Luggage';
import FlightIcon from '@mui/icons-material/Flight';

interface Vacation {
  id: number;
  employeeName: string;
  type: string;
  startDate: string;
  endDate: string;
  reason?: string;
  approvedByName?: string;
}

interface DepartmentVacations {
  departmentName: string;
  vacations: Vacation[];
}

const Vacations = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [vacationsData, setVacationsData] = useState<DepartmentVacations[]>([]);
  const today = new Date();

  useEffect(() => {
    openErpApi.get('vacations/vacationsByDepartment')
      .then((response) => {
        setVacationsData(response.data);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, []);

  const isDateToday = (startDate: string, endDate: string) => {
    const start = new Date(startDate).getTime();
    const end = new Date(endDate).getTime();
    const todayTime = today.setHours(0, 0, 0, 0);

    return todayTime >= start && todayTime <= end;
  };

  return (
    <>
      {isLoading ? (
        <LoadingPage />
      ) : vacationsData.length === 0 ? (
        <Typography variant="h6" align="center">
          No vacations found
        </Typography>
      ) : (
        vacationsData.map((department) => (
          <Grid item xs={12} key={department.departmentName} sx={{ mb: 4 }}>
            <Typography variant="h6" sx={{ mb: 2 }}>
              {department.departmentName}
            </Typography>
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell style={{ fontWeight: 'bold' }}>Employee Name</TableCell>
                    <TableCell style={{ fontWeight: 'bold' }}>Type</TableCell>
                    <TableCell style={{ fontWeight: 'bold' }}>Start Date</TableCell>
                    <TableCell style={{ fontWeight: 'bold' }}>End Date</TableCell>
                    <TableCell style={{ fontWeight: 'bold' }}>Reason</TableCell>
                    <TableCell style={{ fontWeight: 'bold' }}>Approved By</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {department.vacations.map((vacation) => {
                    const isOnVacationToday = isDateToday(vacation.startDate, vacation.endDate);

                    return (
                      <TableRow
                        key={vacation.id}
                        sx={{
                          backgroundColor: isOnVacationToday ? '#A1A1A1' : 'inherit',
                        }}
                      >
                        <TableCell>
                          {vacation.employeeName}
                          {
                            isOnVacationToday
                            ? <>
                              <LuggageIcon fontSize='small' />
                              <FlightIcon fontSize='small' />
                            </>
                            : <></>
                          }
                        </TableCell>
                        <TableCell>{vacation.type}</TableCell>
                        <TableCell>{vacation.startDate}</TableCell>
                        <TableCell>{vacation.endDate}</TableCell>
                        <TableCell>{vacation.reason || 'N/A'}</TableCell>
                        <TableCell>{vacation.approvedByName || 'N/A'}</TableCell>
                      </TableRow>
                    );
                  })}
                </TableBody>
              </Table>
            </TableContainer>
          </Grid>
        ))
      )}
      <SnackbarProvider maxSnack={3} />
    </>
  );
};

export default Vacations;