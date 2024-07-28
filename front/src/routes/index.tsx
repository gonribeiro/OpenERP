import { createBrowserRouter, RouterProvider} from 'react-router-dom'

import { PrivateRoute } from './PrivateRoutes.tsx'

import Login from './../pages/Auth/Login/'
import UpdatePassword from './../pages/Auth/UpdatePassword/'
import UserIndex from './../pages/Auth/User/'
import UserDetails from './../pages/Auth/User/Details'
import RoleIndex from './../pages/Auth/Role/'
import RoleDetails from './../pages/Auth/Role/Details'

import CityIndex from './../pages/Global/City/'
import CityDetails from './../pages/Global/City/Details'
import CompanyIndex from './../pages/Global/Company/'
import CompanyDetails from './../pages/Global/Company/Details'
import CountryIndex from './../pages/Global/Country/'
import CountryDetails from './../pages/Global/Country/Details.tsx'
import Dashboard from './../pages/Global/Dashboard/'
import DepartmentIndex from './../pages/Global/Department/'
import DepartmentDetails from './../pages/Global/Department/Details'
import ErrorPage from './../pages/Global/ErrorPage/'
import StateIndex from './../pages/Global/State/'
import StateDetails from './../pages/Global/State/Details'

import EducationIndex from './../pages/HumanResource/Education/'
import EducationDetails from './../pages/HumanResource/Education/Details'
import EmployeeIndex from './../pages/HumanResource/Employee/'
import EmployeeInactives from './../pages/HumanResource/Employee/Inactives'
import EmployeeDetails from './../pages/HumanResource/Employee/Details'
import JobIndex from './../pages/HumanResource/Job/'
import JobDetails from './../pages/HumanResource/Job/Details'
import JobHistoryIndex from './../pages/HumanResource/JobHistory/'
import JobHistoryDetails from './../pages/HumanResource/JobHistory/Details'
import RemunerationIndex from './../pages/HumanResource/Remuneration/'
import RemunerationDetails from './../pages/HumanResource/Remuneration/Details'
import VacationIndex from './../pages/HumanResource/Vacation/'
import VacationDetails from './../pages/HumanResource/Vacation/Details'
import Birthdays from './../pages/HumanResource/Employee/Birthdays'
import Compensation from './../pages/HumanResource/Employee/Compensation'
import Vacation from './../pages/HumanResource/Employee/Vacation'

const AppRouter = createBrowserRouter([
  {path: "/", element: <Login />, errorElement: <ErrorPage />},
  {
    path: "/",
    element: <PrivateRoute />,
    errorElement: <ErrorPage />,
    children: [
      {path: "/users/", element: <UserIndex />},
      {path: "/users/updatePassword", element: <UpdatePassword />},
      {path: "/users/create", element: <UserDetails />},
      {path: "/users/:id/edit", element: <UserDetails />},
      {path: "/roles/", element: <RoleIndex />},
      {path: "/roles/create", element: <RoleDetails />},
      {path: "/roles/:id/edit", element: <RoleDetails />},

      {path: "/cities/", element: <CityIndex />},
      {path: "/cities/create", element: <CityDetails />},
      {path: "/cities/:id/edit", element: <CityDetails />},
      {path: "/companies/", element: <CompanyIndex />},
      {path: "/companies/create", element: <CompanyDetails />},
      {path: "/companies/:id/edit", element: <CompanyDetails />},
      {path: "/countries/", element: <CountryIndex />},
      {path: "/countries/create", element: <CountryDetails />},
      {path: "/countries/:id/edit", element: <CountryDetails />},
      {path: "/dashboard/", element: <Dashboard />},
      {path: "/departments/", element: <DepartmentIndex />},
      {path: "/departments/create", element: <DepartmentDetails />},
      {path: "/departments/:id/edit", element: <DepartmentDetails />},
      {path: "/states/", element: <StateIndex />},
      {path: "/states/create", element: <StateDetails />},
      {path: "/states/:id/edit", element: <StateDetails />},

      {path: "/employees/:id/educations/", element: <EducationIndex />},
      {path: "/employees/:id/educations/create", element: <EducationDetails />},
      {path: "/employees/:id/educations/:educationId/edit", element: <EducationDetails />},
      {path: "/employees", element: <EmployeeIndex />},
      {path: "/employees/inactives", element: <EmployeeInactives />},
      {path: "/employees/create", element: <EmployeeDetails />},
      {path: "/employees/:id/edit", element: <EmployeeDetails />},
      {path: "/jobs/", element: <JobIndex />},
      {path: "/jobs/create", element: <JobDetails />},
      {path: "/jobs/:id/edit", element: <JobDetails />},
      {path: "/employees/:id/jobHistories/", element: <JobHistoryIndex />},
      {path: "/employees/:id/jobHistories/create", element: <JobHistoryDetails />},
      {path: "/employees/:id/jobHistories/:jobHistoryId/edit", element: <JobHistoryDetails />},
      {path: "/employees/:id/remunerations/", element: <RemunerationIndex />},
      {path: "/employees/:id/remunerations/create", element: <RemunerationDetails />},
      {path: "/employees/:id/remunerations/:remunerationId/edit", element: <RemunerationDetails />},
      {path: "/employees/:id/vacations/", element: <VacationIndex />},
      {path: "/employees/:id/vacations/create", element: <VacationDetails />},
      {path: "/employees/:id/vacations/:vacationId/edit", element: <VacationDetails />},
      {path: "/employees/birthdays", element: <Birthdays />},
      {path: "/employees/compensation", element: <Compensation />},
      {path: "/employees/vacation", element: <Vacation />},
    ]
  }
])

const Routes = () => {
  return (
    <RouterProvider router={AppRouter} />
  )
}

export default Routes