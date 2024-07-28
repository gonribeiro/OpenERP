import { Fragment, useState } from 'react';
import { Link } from 'react-router-dom';

import Button from '@mui/material/Button';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import Fade from '@mui/material/Fade';
import { Box, Collapse, List, ListItemButton, ListItemText } from '@mui/material';
import { ExpandLess, ExpandMore } from '@mui/icons-material';

interface MenuEmployeeProps {
  color?: string;
}

interface ResponsiveMenuProps {
  closeToggleDrawer?: (event: React.KeyboardEvent | React.MouseEvent) => void;
  closeMenu?: () => void;
}

export function EmployeeMenuList({closeToggleDrawer, closeMenu}: ResponsiveMenuProps) {
  const handleToggleDrawer = (e: React.KeyboardEvent | React.MouseEvent) => {
    if(closeToggleDrawer)
      closeToggleDrawer(e);
  };

  const handleCloseMenu = () => {
    if(closeMenu)
      closeMenu();
  };

  return (
    <Fragment>
      <MenuItem
        onClick={(e) => {handleToggleDrawer(e); handleCloseMenu()}}
        component={Link}
        to="/employees"
      >
        Active
      </MenuItem>
      <MenuItem
        onClick={(e) => {handleToggleDrawer(e); handleCloseMenu()}}
        component={Link}
        to="/employees/inactives"
      >
        Inactive
      </MenuItem>
      <MenuItem
        onClick={(e) => {handleToggleDrawer(e); handleCloseMenu()}}
        component={Link}
        to="/employees/birthdays"
      >
        Birthdays
      </MenuItem>
      <MenuItem
        onClick={(e) => {handleToggleDrawer(e); handleCloseMenu()}}
        component={Link}
        to="/employees/compensation"
      >
        Compensation
      </MenuItem>
      <MenuItem
        onClick={(e) => {handleToggleDrawer(e); handleCloseMenu()}}
        component={Link}
        to="/employees/Vacation"
      >
        Vacation / Leave
      </MenuItem>
    </Fragment>
  );
}

export function MenuEmployee({color}: MenuEmployeeProps) {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);
  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <div>
      <Button
        id="fade-button"
        aria-controls={open ? 'fade-menu' : undefined}
        aria-haspopup="true"
        aria-expanded={open ? 'true' : undefined}
        onClick={handleClick}
        sx={{ color: color ?? 'white', display: 'flex', alignItems: 'center' }}
      >
        <Box>Employees</Box>
        {open ? <ExpandLess /> : <ExpandMore />}
      </Button>
      <Menu
        id="fade-menu"
        MenuListProps={{
          'aria-labelledby': 'fade-button',
        }}
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
        TransitionComponent={Fade}
      >
        <EmployeeMenuList closeMenu={handleClose}/>
      </Menu>
    </div>
  );
}

export function ResponsiveMenuEmployee({closeToggleDrawer}: ResponsiveMenuProps) {
  const [open, setOpen] = useState(false);

  const handleToggleDrawer = (e: React.KeyboardEvent | React.MouseEvent) => {
    if(closeToggleDrawer)
      closeToggleDrawer(e);
  };

  return (
    <div>
      <ListItemButton onClick={() => setOpen(!open)}>
        <ListItemText primary="Employee" />
        {open ? <ExpandLess /> : <ExpandMore />}
      </ListItemButton>
      <Collapse
        in={open}
        timeout="auto"
        unmountOnExit
        sx={{ backgroundColor: "rgba(0, 0, 0, 0.1)" }}
      >
        <List component="div" disablePadding>
          <EmployeeMenuList closeToggleDrawer={handleToggleDrawer}/>
        </List>
      </Collapse>
    </div>
  );
}