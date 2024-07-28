import { Fragment, useEffect, useState } from 'react';
import { SnackbarProvider } from 'notistack';

import openErpApi from '../../../../services/OpenErpApi';
import LoadingPage from '../../../../utils/LoadingPage';

import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography } from '@mui/material';

interface EmployeeCompensationProps {
  employeeFullName: string;
  totalCompensation: number;
  sharedCompensationByDepartment: number;
}

interface DepartmentCompensationViewModel {
  departmentName: string;
  totalCompensation: number;
  sharedCompensation: number;
  users: EmployeeCompensationProps[];
}

const Compensation = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [compensationsByDepartment, setCompensationsByDepartment] = useState<DepartmentCompensationViewModel[]>([]);

  console.log(compensationsByDepartment)

  useEffect(() => {
    openErpApi.get('departments/compensationByDepartment')
      .then((response) => {
        setCompensationsByDepartment(response.data);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, []);

  return (
    <>
      {isLoading ? (
        <LoadingPage />
      ) : compensationsByDepartment.length === 0 ? (
        <Typography variant="h6" align="center">
          No compensation data available
        </Typography>
      ) : (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell style={{ fontWeight: 'bold' }}>
                  <Typography variant="h6">Department</Typography>
                </TableCell>
                <TableCell style={{ fontWeight: 'bold' }}>
                  <Typography variant="h6">Employees</Typography>
                </TableCell>
                <TableCell style={{ fontWeight: 'bold' }}>
                  <Typography variant="h6">Total Compensation</Typography>
                </TableCell>
                <TableCell style={{ fontWeight: 'bold' }}>
                  <Typography variant="h6">Shared Compensation</Typography>
                </TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {compensationsByDepartment.map((department) => (
                <Fragment key={department.departmentName}>
                  <TableRow>
                    <TableCell>
                      <Typography variant="h6">
                        {department.departmentName}
                      </Typography>
                    </TableCell>
                    <TableCell></TableCell>
                    <TableCell>
                      <Typography variant="h6">
                        ${department.totalCompensation.toFixed(2)}
                      </Typography>
                    </TableCell>
                    <TableCell>
                      <Typography variant="h6">
                        ${department.sharedCompensation.toFixed(2)}
                      </Typography>
                    </TableCell>
                  </TableRow>
                  {department.users.length !== 0 && department.users.map((compensation) => (
                    <TableRow key={compensation.employeeFullName}>
                      <TableCell></TableCell>
                      <TableCell>
                        {compensation.employeeFullName}
                      </TableCell>
                      <TableCell>
                        ${compensation.totalCompensation ? compensation.totalCompensation.toFixed(2) : "0.00"}
                      </TableCell>
                      <TableCell>
                        ${compensation.sharedCompensationByDepartment.toFixed(2)}
                      </TableCell>
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

export default Compensation;