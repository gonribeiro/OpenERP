import { Fragment, useEffect, useState } from 'react';
import { SnackbarProvider } from 'notistack';

import openErpApi from '../../../../services/OpenErpApi';
import LoadingPage from '../../../../utils/LoadingPage';

import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography } from '@mui/material';

interface StateProps {
  id: string;
  name: string;
  birthdate: string;
}

interface BirthdaysByMonth {
  [key: string]: StateProps[];
}

const monthNames = [
  "January", "February", "March", "April", "May", "June", 
  "July", "August", "September", "October", "November", "December"
];

const Birthdays = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [birthdays, setBirthdays] = useState<BirthdaysByMonth>({});

  useEffect(() => {
    openErpApi.get('employees/birthdays')
      .then((response) => {
        setBirthdays(response.data);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, []);

  const getMonthName = (monthNumber: string) => {
    const monthIndex = parseInt(monthNumber, 10) - 1;
    return monthNames[monthIndex];
  };

  return (
    <>
      {isLoading ? (
        <LoadingPage />
      ) : Object.keys(birthdays).length === 0 ? (
        <Typography variant="h6" align="center">
          No birthdays found
        </Typography>
      ) : (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell style={{ fontWeight: 'bold' }}>Month</TableCell>
                <TableCell style={{ fontWeight: 'bold' }}>Employee Name</TableCell>
                <TableCell style={{ fontWeight: 'bold' }}>Birthdate</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {Object.entries(birthdays).map(([month, birthdaysList]) => (
                <Fragment key={month}>
                  {birthdaysList.map((birthday) => (
                    <TableRow key={birthday.id}>
                      <TableCell>{getMonthName(month)}</TableCell>
                      <TableCell>{birthday.name}</TableCell>
                      <TableCell>{birthday.birthdate}</TableCell>
                    </TableRow>
                  ))}
                </Fragment>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      )}
      <SnackbarProvider maxSnack={3} />
    </>
  );
};

export default Birthdays;