import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';


import openErpApi from '../../../services/OpenErpApi';
import LoadingPage from '../../../utils/LoadingPage';

import {
  PieChart, Pie, Cell, Tooltip, Legend, ResponsiveContainer,
  BarChart, Bar, XAxis, YAxis, CartesianGrid, LabelList
} from 'recharts';

import { Link as MuiLink, Typography, Grid, Paper } from '@mui/material';
import CakeIcon from '@mui/icons-material/Cake';
import CelebrationIcon from '@mui/icons-material/Celebration';

interface DepartmentCompensation {
  departmentName: string;
  totalCompensation: number | null;
  sharedCompensation: number | null;
}

interface EmployeeCountByDepartment {
  departmentName: string;
  employeeCount: number;
}

const ChartCOLORS = [
  '#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#AA00FF', '#FF0000',
  '#00BFFF', '#32CD32', '#FFD700', '#FF4500', '#6A5ACD', '#FF1493',
  '#00FF7F', '#7FFF00', '#FF6347', '#40E0D0', '#FF69B4', '#800080',
  '#4682B4', '#3CB371', '#FFA07A', '#DDA0DD', '#20B2AA', '#5F9EA0',
  '#F08080', '#D3D3D3', '#ADD8E6', '#F5DEB3', '#F5F5DC', '#FFB6C1',
  '#E6E6FA', '#FFFACD', '#FAEBD7', '#D3D3D3', '#F0E68C', '#E9967A',
  '#8A2BE2', '#A52A2A', '#DEB887', '#5F9EA0', '#7FFF00', '#D2691E',
  '#FF7F50', '#6495ED', '#FFF8DC', '#DC143C', '#00FFFF', '#00008B',
  '#008080', '#ADFF2F', '#FF69B4', '#CD5C5C', '#4B0082', '#800000'
];

const Dashboard = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [compensationByDepartment, setCompensationByDepartment] = useState<DepartmentCompensation[]>([]);
  const [employeeCountByDepartment, setEmployeeCountByDepartment] = useState<EmployeeCountByDepartment[]>([]);

  useEffect(() => {
    const promises = [
      openErpApi.get(`/departments/compensationByDepartment`),
      openErpApi.get(`/departments/employeeCount`),
    ];

    Promise.all(promises)
      .then(([compensationByDepartment, employeeCountByDepartment]) => {
        setCompensationByDepartment(compensationByDepartment.data);
        setEmployeeCountByDepartment(employeeCountByDepartment.data);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, []);

  return (
    <>
      {isLoading ? <LoadingPage /> : (
        <Grid container spacing={3} padding={2}>
          <Grid item xs={12}>
            <Paper elevation={2} style={{ padding: '16px' }}>
              <Typography variant="h6">
                <MuiLink underline="hover" component={Link} to="/employees/birthdays">
                  Birthdays <CelebrationIcon fontSize='small' /><CakeIcon fontSize='small' />
                </MuiLink>
              </Typography>
            </Paper>
          </Grid>
          {
            compensationByDepartment.length !== 0
              ? <>
              <Grid item xs={12} md={6}>
                <Paper elevation={2} style={{ padding: '16px' }}>
                  <Typography variant="h6" align="center" gutterBottom sx={{mb: 4}}>
                    Total Compensation by Department
                  </Typography>
                  <ResponsiveContainer width="100%" height={450}>
                    <PieChart>
                      <Pie
                        data={compensationByDepartment}
                        dataKey="totalCompensation"
                        nameKey="departmentName"
                        cx="50%"
                        cy="50%"
                        outerRadius={120}
                        fill="#8884d8"
                        label
                      >
                        {compensationByDepartment.map((entry, index) => (
                          <Cell key={`cell-${index}`} fill={ChartCOLORS[index % ChartCOLORS.length]} />
                        ))}
                      </Pie>
                      <Tooltip />
                      <Legend
                        layout="horizontal"
                        verticalAlign="bottom"
                        align="center"
                        wrapperStyle={{ fontSize: '12px' }}
                      />
                    </PieChart>
                  </ResponsiveContainer>
                </Paper>
              </Grid>
              <Grid item xs={12} md={6}>
                <Paper elevation={2} style={{ padding: '16px' }}>
                  <Typography variant="h6" align="center" gutterBottom>
                    Shared Compensation by Department
                  </Typography>
                  <Typography variant="body2" align="center" gutterBottom>
                    For employees working in multiple departments
                  </Typography>
                  <ResponsiveContainer width="100%" height={450}>
                    <PieChart>
                      <Pie
                        data={compensationByDepartment}
                        dataKey="sharedCompensation"
                        nameKey="departmentName"
                        cx="50%"
                        cy="50%"
                        outerRadius={120}
                        fill="#8884d8"
                        label
                      >
                        {compensationByDepartment.map((entry, index) => (
                          <Cell key={`cell-${index}`} fill={ChartCOLORS[index % ChartCOLORS.length]} />
                        ))}
                      </Pie>
                      <Tooltip />
                      <Legend
                        layout="horizontal"
                        verticalAlign="bottom"
                        align="center"
                        wrapperStyle={{ fontSize: '12px' }}
                      />
                    </PieChart>
                  </ResponsiveContainer>
                </Paper>
              </Grid>
            </>
            : <Grid item xs={12}>
              <Paper elevation={2} style={{ padding: '16px', textAlign: 'center' }}>
                <Typography variant="body1">
                  No compensation data is available. Please register new employees,
                  enter their compensation details, and assign them to a department.
                </Typography>
              </Paper>
            </Grid>
          }
          {
            employeeCountByDepartment.length !== 0
            ? <Grid item xs={12} md={12}>
              <Paper elevation={2} style={{ padding: '16px' }}>
                <Typography variant="h6" align="center" gutterBottom>
                  Total Employees by Department
                </Typography>
                <ResponsiveContainer width="100%" height={350}>
                  <BarChart
                    data={employeeCountByDepartment}
                    margin={{ top: 20, right: 30, left: 20, bottom: 50 }} // Aumentando o margin inferior
                  >
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis 
                      dataKey="departmentName" 
                      angle={-45} 
                      textAnchor="end" 
                      tick={{ fontSize: 12 }} // Reduzindo o tamanho da fonte dos rótulos
                    />
                    <YAxis />
                    <Tooltip />
                    <Legend verticalAlign="top" height={36} /> {/* Movendo a legenda para o topo */}
                    <Bar dataKey="employeeCount" fill="#0088FE" name="Total Employees">
                      <LabelList dataKey="employeeCount" position="top" />
                    </Bar>
                    <Bar dataKey="onVacationCount" fill="#00C49F" name="On Vacation or Leave">
                      <LabelList dataKey="onVacationCount" position="top" />
                    </Bar>
                  </BarChart>
                </ResponsiveContainer>
              </Paper>
            </Grid>
            : <Grid item xs={12}>
              <Paper elevation={2} style={{ padding: '16px', textAlign: 'center' }}>
                <Typography variant="body1">
                  No data available for total employees by department.
                  Please register new employees and assign them to a department.
                </Typography>
              </Paper>
            </Grid>
          }
        </Grid>
      )}
    </>
  );
};

export default Dashboard;