import { Fragment, useEffect, useState } from 'react';
import { useForm, SubmitHandler, useFieldArray } from 'react-hook-form';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';

import openErpApi from '../../../services/OpenErpApi';
import LoadingPage from '../../../utils/LoadingPage';

import SaveButton from '../../../components/Form/SaveButton';
import PageTitle from '../../../components/Form/PageTitle';
import BackButton from '../../../components/Form/BackButton';
import ModalDelete from '../../../components/Form/ModalDelete';
import InputText from '../../../components/Form/InputText';
import SelectAutocomplete from '../../../components/Form/SelectAutocomplete';

import { Button, Grid, Typography } from '@mui/material';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';

interface SelectAutoCompleteProps {
  id: string;
  name: string;
}

interface ContactInputProps {
  id: number;
  type: string;
  information: string;
  contactName: string | null;
  contactRelationType: string;
}

interface CompanyInputProps {
  legalName: string;
  tradeName: string | null;
  type: string;
  address: string | null;
  zipCode: string | null;
  cityId: number | null;
  productAndServiceDescription: string | null;
  contacts: ContactInputProps[],
}

const Details = () => {
  const [isLoading, setIsLoading] = useState(true);
  const { handleSubmit, control, reset, formState: { isSubmitting }, setValue } = useForm<CompanyInputProps>({
    defaultValues: {
      legalName: '',
      tradeName: null,
      type: '',
      address: null,
      zipCode: null,
      cityId: null,
      contacts: [],
    }
  });
  const { fields: contactFields, append: appendContact, remove: removeContact } = useFieldArray({
    control,
    name: 'contacts',
  });
  const [cities, setCities] = useState([]);
  const [companyType, setCompanyType] = useState<SelectAutoCompleteProps[]>([]);
  const [contactTypes, setContactTypes] = useState<SelectAutoCompleteProps[]>([]);
  const location = useLocation();
  const navigate = useNavigate();
  const { id } = useParams();

  const handleContact = (index?: number) => {
    if (index == undefined) {
      // 999*999 Prevents passing an existing Id during the creation of a new record
      appendContact({ id: contactFields.length + 999*999, type: '', information: '', contactName: '', contactRelationType: 'Employee' });
    } else {
      removeContact(index);
    }
  };

  useEffect(() => {
    const promises = [
      openErpApi.get(`cities/`),
      openErpApi.get(`enums/company-types`),
      openErpApi.get(`enums/contact-types`)
    ];

    if (location.pathname !== '/companies/create')
      promises.push(openErpApi.get(`companies/${id}`));

    Promise.all(promises)
      .then(([cities, companyTypes, contactTypes, company]) => {
        setCities(cities.data);
        setCompanyType(companyTypes.data);
        setContactTypes(contactTypes.data);

        if (location.pathname !== '/companies/create')
          reset(company.data);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, [location.pathname, id]);

  const onSubmit: SubmitHandler<CompanyInputProps> = async (data) => {
    if (location.pathname === '/companies/create') {
      await openErpApi.post(`/companies`, data)
        .then(response => {
          navigate(`/${response.data.redirectTo}`);
        });
    } else {
      await openErpApi.put(`companies/${id}`, data)
        .then(response => {
          const updatedContacts: ContactInputProps[] = response.data.employee.contacts;

          updatedContacts.forEach((contact, index) => {
            setValue(`contacts.${index}.id`, contact.id);
          });
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
            <Grid item xs={6} md={6}>
              <PageTitle name={"Company"} />
            </Grid>
            <Grid item xs={6} md={6} container justifyContent="flex-end">
              <BackButton url={'/companies'} />
            </Grid>
            <Grid item xs={12} md={12}>
              <InputText
                name="legalName"
                control={control}
                rules={{required: true, minLength: 3, maxLength: 120}}
                label="Legal Name"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <InputText
                name="tradeName"
                control={control}
                rules={{required: true, minLength: 3, maxLength: 120}}
                label="Trade Name"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <SelectAutocomplete
                name="type"
                control={control}
                rules={{ required: true }}
                options={companyType}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <InputText
                name="address"
                control={control}
              />
            </Grid>
            <Grid item xs={12} md={3}>
              <InputText
                name="zipCode"
                control={control}
              />
            </Grid>
            <Grid item xs={12} md={3}>
              <SelectAutocomplete
                name="cityId"
                control={control}
                options={cities}
                label="City"
              />
            </Grid>
            <Grid item xs={12} md={12}>
              <InputText
                name="productAndServiceDescription"
                control={control}
                label="Product And Service Description"
                multiline={4}
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
                <Grid item xs={12} md={3}>
                  <SelectAutocomplete
                    name={`contacts.${index}.type`}
                    control={control}
                    rules={{ required: true }}
                    options={contactTypes}
                    label="Type"
                  />
                </Grid>
                <Grid item xs={12} md={4}>
                  <InputText
                    name={`contacts.${index}.information`}
                    control={control}
                    rules={{required: true, minLength: 3, maxLength: 120}}
                    label="Information"
                  />
                </Grid>
                <Grid item xs={12} md={4}>
                  <InputText
                    name={`contacts.${index}.contactName`}
                    control={control}
                    rules={{minLength: 3, maxLength: 120}}
                    label="Employee"
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
            <Grid item xs={6} md={6}>
              <SaveButton loading={isSubmitting} />
            </Grid>
            {
              location.pathname !== '/companies/create'
              ? <Grid item xs={6} md={6} container justifyContent="flex-end">
                <ModalDelete
                  url={`companies/${id}`}
                  title={'Company'}
                  text={"Are you sure you want to delete this company?\
                  The data cannot be restored."}
                />
              </Grid>
              : <></>
            }
          </Grid>
          <SnackbarProvider/>
        </form>
      }
    </>
  );
};

export default Details;