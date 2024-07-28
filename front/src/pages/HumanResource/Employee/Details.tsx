import { Fragment, useEffect, useState } from 'react';

import { useForm, useFieldArray, SubmitHandler, Controller } from 'react-hook-form';
import { useLocation, useNavigate, useParams, Link } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';

import openErpApi from '../../../services/OpenErpApi';
import LoadingPage from '../../../utils/LoadingPage';

import SaveButton from '../../../components/Form/SaveButton';
import PageTitle from '../../../components/Form/PageTitle';
import BackButton from '../../../components/Form/BackButton';
import InputText from '../../../components/Form/InputText';
import SelectAutocomplete from '../../../components/Form/SelectAutocomplete';
import PageSubTitle from '../../../components/Form/PageSubTitle';
import InputDate from '../../../components/Form/InputDate';
import SelectAutocompleteMultiple from '../../../components/Form/SelectAutocompleteMultiple';
import Avatar from '../../../components/Form/Avatar';

import { Alert, Button, FormControlLabel, Grid, Switch, Typography } from '@mui/material';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';

interface ContactInputProps {
  id: number;
  type: string;
  information: string;
}

interface RelativeInputProps {
  id: number;
  type: string;
  information: string;
  contactName: string;
  contactRelationType: string;
}

interface EmployeeInputProps {
  id: number;
  photo: string | null;
  photoId: string | null;
  inactiveDate: Date | null;
  firstName: string;
  lastName: string;
  birthdate: string;
  placeOfBirth: string | null
  maritalStatus: string;
  nationalityId: number;
  departmentIds: number[];
  address: string;
  zipCode: string;
  CityId: number;
  socialSecurityNumber: string;
  passportNumber: string | null;
  driveLicenceNumber: string | null;
  bankId: number | null;
  accountNumber: string | null;
  routingNumber: string | null;
  contacts: ContactInputProps[];
  relatives: RelativeInputProps[];
}

const Details = () => {
  const [isLoading, setIsLoading] = useState(true);
  const { handleSubmit, control, reset, formState: { errors, isSubmitting }, getValues } = useForm<EmployeeInputProps>({
    defaultValues: {
      id: 0,
      photo: null,
      photoId: null,
      inactiveDate: null,
      firstName: '',
      lastName: '',
      birthdate: '',
      placeOfBirth: null,
      maritalStatus: '',
      nationalityId: 0,
      departmentIds: [],
      address: '',
      zipCode: '',
      CityId: 0,
      socialSecurityNumber: '',
      passportNumber: null,
      driveLicenceNumber: null,
      bankId: null,
      accountNumber: null,
      routingNumber: null,
      contacts: [],
      relatives: [],
    }
  });
  const { fields: contactFields, append: appendContact, remove: removeContact } = useFieldArray({
    control,
    name: 'contacts',
  });
  const { fields: relativeFields, append: appendRelative, remove: removeRelative } = useFieldArray({
    control,
    name: 'relatives',
  });
  const [employeeIsInactive, setEmployeeIsInactive] = useState<any>(null);
  const [maritalStatus, setMaritalStatus] = useState([]);
  const [nationalities, setNationalities] = useState([]);
  const [contactTypes, setContactTypes] = useState([]);
  const [departments, setDepartments] = useState([]);
  const [cities, setCities] = useState([]);
  const [banks, setBanks] = useState([]);
  const [contactRelationTypes, setContactRelationTypes] = useState([]);
  const location = useLocation();
  const navigate = useNavigate();
  const { id } = useParams();

  const handleContact = (index?: number) => {
    if (index == undefined) {
      // 999*999 Prevents passing an existing Id during the creation of a new record
      appendContact({ id: contactFields.length + 999*999, type: '', information: '' });
    } else {
      removeContact(index);
    }
  };

  const handleRelative = (index?: number) => {
    if (index == undefined) {
      // 999*999 Prevents passing an existing Id during the creation of a new record
      appendRelative({ id: contactFields.length + 999*999, type: 'Phone', information: '', contactName: '', contactRelationType: '' });
    } else {
      removeRelative(index);
    }
  };

  useEffect(() => {
    const promises = [
      openErpApi.get(`enums/maritalStatus-types`),
      openErpApi.get(`countries/nationalities`),
      openErpApi.get(`enums/contact-types`),
      openErpApi.get(`departments/`),
      openErpApi.get(`cities/`),
      openErpApi.get(`companies/Bank`),
      openErpApi.get(`enums/contactRelation-types`)
    ];

    if (location.pathname !== '/employees/create')
      promises.push(openErpApi.get(`employees/${id}`));

    Promise.all(promises)
      .then(([
        maritalStatusTypes,
        nationalities,
        contactTypes,
        departments,
        cities,
        banks,
        contactRelationTypes,
        employee,
      ]) => {
        setMaritalStatus(maritalStatusTypes.data);
        setNationalities(nationalities.data);
        setContactTypes(contactTypes.data);
        setDepartments(departments.data);
        setCities(cities.data);
        setBanks(banks.data);
        setContactRelationTypes(contactRelationTypes.data
            .filter((type: ContactInputProps) => type.id.toString() !== "Employee"));

        if (location.pathname !== '/employees/create') {
          reset(employee.data);
          setEmployeeIsInactive(employee.data.inactiveDate);
        }
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, [location.pathname, id]);

  const onSubmit: SubmitHandler<EmployeeInputProps> = async (data) => {
    if (location.pathname === '/employees/create') {
      await openErpApi.post(`/employees`, data)
        .then(response => {
          navigate(`/${response.data.redirectTo}`);
        });
    } else {
      await openErpApi.put(`employees/${id}`, data)
        .then(() => {
          setEmployeeIsInactive(data.inactiveDate);
        });
    }
  };

  return (
    <>
      {
        isLoading
          ? <LoadingPage />
          : <form onSubmit={handleSubmit(onSubmit)}>
          <Grid container spacing={2}>
            { employeeIsInactive
              ? <Grid item xs={12} md={12}>
                <Alert severity="info">{`Employee inactive since ${employeeIsInactive}`}</Alert>
              </Grid>
              : <></>
            }
            {
                location.pathname !== '/employees/create'
                ? <Grid item xs={12} md={12} container justifyContent="center">
                  <Avatar employeeId={getValues("id")} initialAvatarId={getValues("photoId")} initialAvatar={getValues("photo")} />
                </Grid>
                : <></>
            }
            <Grid item xs={6} md={6}>
              <PageTitle name={"Employee"} />
            </Grid>
            <Grid item xs={6} md={6} container justifyContent="flex-end">
              <BackButton
                url={employeeIsInactive === null || typeof employeeIsInactive === 'undefined'
                  ? '/employees'
                  : '/employees/inactives'}
              />
            </Grid>
            <Grid item xs={12} md={10}>
              <Typography variant="h6" gutterBottom>
                Personal Information
                {
                  location.pathname !== '/employees/create'
                  ? <>
                    <Button
                      variant="outlined"
                      color="primary"
                      size='small'
                      sx={{ marginLeft: "5px"}}
                      component={Link}
                      to={`/employees/${id}/jobHistories`}
                    >
                      Job History
                    </Button>
                    <Button
                      variant="outlined"
                      color="primary"
                      size='small'
                      sx={{ marginLeft: "5px"}}
                      component={Link}
                      to={`/employees/${id}/remunerations`}
                    >
                      Remuneration
                    </Button>
                    <Button
                      variant="outlined"
                      color="primary"
                      size='small'
                      sx={{ marginLeft: "5px"}}
                      component={Link}
                      to={`/employees/${id}/educations`}
                    >
                      Education
                    </Button>
                    <Button
                      variant="outlined"
                      color="primary"
                      size='small'
                      sx={{ marginLeft: "5px"}}
                      component={Link}
                      to={`/employees/${id}/vacations`}
                    >
                      Vacation / Leave
                    </Button>
                  </>
                  : <></>
                }
              </Typography>
            </Grid>
            <Grid item xs={12} md={2} container justifyContent="flex-end">
              {
                location.pathname !== '/employees/create'
                ? <Controller
                  name="inactiveDate"
                  control={control}
                  render={({ field }) => (
                    <FormControlLabel
                      control={
                        <Switch
                          checked={!!field.value}
                          onChange={(event) => field.onChange(event.target.checked
                            ? new Date().toLocaleDateString('en-CA', { year: 'numeric', month: '2-digit', day: '2-digit' })
                            : null
                          )}
                        />
                      }
                      label={'Inactive'}
                    />
                  )}
                />
                : <></>
              }
            </Grid>
            <Grid item xs={12} md={6}>
              <InputText
                name="firstName"
                control={control}
                rules={{required: true, minLength: 3, maxLength: 120}}
                label="First Name"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <InputText
                name="lastName"
                control={control}
                rules={{required: true, minLength: 3, maxLength: 120}}
                label="Last Name"
              />
            </Grid>
            <Grid item xs={12} md={3}>
              <InputDate
                name="birthdate"
                control={control}
                rules={{required: true}}
              />
            </Grid>
            <Grid item xs={12} md={3}>
              <SelectAutocomplete
                name="maritalStatus"
                control={control}
                rules={{ required: true }}
                options={maritalStatus}
                label="Marital Status"
              />
            </Grid>
            <Grid item xs={12} md={3}>
              <SelectAutocomplete
                name="nationalityId"
                control={control}
                rules={{ required: true }}
                options={nationalities}
                label="Nationality"
              />
            </Grid>
            <Grid item xs={12} md={3}>
              <InputText
                name="placeOfBirth"
                control={control}
                rules={{minLength: 3, maxLength: 120}}
                label={'Place of Birth'}
              />
            </Grid>
            <Grid item xs={12} md={12}>
              <Typography variant="h6" gutterBottom>
                Contacts
                <Button
                  variant="outlined"
                  color="primary"
                  size='small'
                  onClick={() => handleContact()}
                  sx={{ marginLeft: "5px"}}
                >
                  New
                </Button>
              </Typography>
            </Grid>
            {contactFields.map((field, index) => (
              <Fragment key={field.id}>
                <input type="hidden" name={`contacts.${index}.id`} defaultValue={field.id}/>
                <Grid item xs={12} md={5}>
                  <SelectAutocomplete
                    name={`contacts.${index}.type`}
                    control={control}
                    rules={{ required: true }}
                    options={contactTypes}
                    label="Type"
                  />
                </Grid>
                <Grid item xs={12} md={6}>
                  <InputText
                    name={`contacts.${index}.information`}
                    control={control}
                    rules={{required: true, minLength: 3, maxLength: 120}}
                    label="Information"
                  />
                </Grid>
                <Grid item xs={12} md={1} container justifyContent="center">
                  <Button
                    variant="text"
                    color="error"
                    size='large'
                    onClick={() => handleContact(index)}
                  >
                    <DeleteOutlineIcon />
                  </Button>
                </Grid>
              </Fragment>
            ))}
            <Grid item xs={12} md={12}>
              <PageSubTitle name={"Departments"} />
            </Grid>
            <Grid item xs={12} md={12}>
              <SelectAutocompleteMultiple
                name="departmentIds"
                control={control}
                options={departments}
                errors={errors}
                label="Departments"
              />
            </Grid>
            <Grid item xs={12} md={12}>
              <PageSubTitle name={"Address"} />
            </Grid>
            <Grid item xs={12} md={4}>
              <InputText
                name="address"
                control={control}
                rules={{required: true, minLength: 3, maxLength: 120}}
              />
            </Grid>
            <Grid item xs={12} md={4}>
              <InputText
                name="zipCode"
                control={control}
                rules={{required: true, minLength: 3, maxLength: 120}}
              />
            </Grid>
            <Grid item xs={12} md={4}>
              <SelectAutocomplete
                name="cityId"
                control={control}
                rules={{required: true}}
                options={cities}
                label="City"
              />
            </Grid>
            <Grid item xs={12} md={12}>
              <PageSubTitle name={"Documents"} />
            </Grid>
            <Grid item xs={12} md={4}>
              <InputText
                name="socialSecurityNumber"
                control={control}
                rules={{required: true, minLength: 6, maxLength: 120}}
                label="Social Security Number"
              />
            </Grid>
            <Grid item xs={12} md={4}>
              <InputText
                name="passportNumber"
                control={control}
                label="Passport Number"
              />
            </Grid>
            <Grid item xs={12} md={4}>
              <InputText
                name="driverLicenseNumber"
                control={control}
                label="Driver License Number"
              />
            </Grid>
            <Grid item xs={12} md={12}>
              <PageSubTitle name={"Bank Details"} />
            </Grid>
            <Grid item xs={12} md={4}>
              <SelectAutocomplete
                name="bankId"
                control={control}
                options={banks}
                label="Bank"
              />
            </Grid>
            <Grid item xs={12} md={4}>
              <InputText
                name="accountNumber"
                control={control}
                label="Account Number"
              />
            </Grid>
            <Grid item xs={12} md={4}>
              <InputText
                name="routingNumber"
                control={control}
                label="Routing Number"
              />
            </Grid>
            <Grid item xs={12} md={12}>
              <Typography variant="h6" gutterBottom>
                Relatives
                <Button
                  variant="outlined"
                  color="primary"
                  size='small'
                  onClick={() => handleRelative()}
                  sx={{ marginLeft: "5px"}}
                >
                  New
                </Button>
              </Typography>
            </Grid>
            {relativeFields.map((field, index) => (
              <Fragment key={field.id}>
                <Grid item xs={12} md={3}>
                  <SelectAutocomplete
                    name={`relatives.${index}.contactRelationType`}
                    control={control}
                    rules={{ required: true }}
                    options={contactRelationTypes}
                    label="Relation"
                  />
                </Grid>
                <Grid item xs={12} md={4}>
                  <InputText
                    name={`relatives.${index}.contactName`}
                    control={control}
                    rules={{required: true, minLength: 3, maxLength: 120}}
                    label="Name"
                  />
                </Grid>
                <Grid item xs={12} md={4}>
                  <InputText
                    name={`relatives.${index}.information`}
                    control={control}
                    rules={{required: true, minLength: 3, maxLength: 120}}
                    label="Phone"
                  />
                </Grid>
                <Grid item xs={12} md={1} container justifyContent="center">
                  <Button
                    variant="text"
                    color="error"
                    size='large'
                    onClick={() => handleRelative(index)}
                  >
                    <DeleteOutlineIcon />
                  </Button>
                </Grid>
              </Fragment>
            ))}
            <Grid item xs={6} md={6}>
              <SaveButton loading={isSubmitting} />
            </Grid>
          </Grid>
          <SnackbarProvider/>
        </form>
      }
    </>
  );
};

export default Details;