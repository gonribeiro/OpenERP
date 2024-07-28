import { Fragment, useState } from 'react';
import { Link } from 'react-router-dom';

import Button from '@mui/material/Button';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import Fade from '@mui/material/Fade';
import { Box, Collapse, Divider, List, ListItemButton, ListItemText } from '@mui/material';
import { ExpandLess, ExpandMore } from '@mui/icons-material';

interface MenuSystemProps {
  color?: string;
}

interface ResponsiveMenuProps {
  closeToggleDrawer?: (event: React.KeyboardEvent | React.MouseEvent) => void;
  closeMenu?: () => void;
}

export function SystemMenuList({closeToggleDrawer, closeMenu}: ResponsiveMenuProps) {
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
        to="/users"
      >
        User Manager
      </MenuItem>
      <MenuItem
        onClick={(e) => {handleToggleDrawer(e); handleCloseMenu()}}
        component={Link}
        to="/roles"
      >
        Roles
      </MenuItem>
      <Divider />
      <MenuItem
        onClick={(e) => {handleToggleDrawer(e); handleCloseMenu()}}
        component={Link}
        to="/departments"
      >
        Departments
      </MenuItem>
      <MenuItem
        onClick={(e) => {handleToggleDrawer(e); handleCloseMenu()}}
        component={Link}
        to="/jobs"
      >
        Jobs
      </MenuItem>
      <Divider />
      <MenuItem
        onClick={(e) => {handleToggleDrawer(e); handleCloseMenu()}}
        component={Link}
        to="/countries"
      >
        Countries
      </MenuItem>
      <MenuItem
        onClick={(e) => {handleToggleDrawer(e); handleCloseMenu()}}
        component={Link}
        to="/states"
      >
        States
      </MenuItem>
      <MenuItem
        onClick={(e) => {handleToggleDrawer(e); handleCloseMenu()}}
        component={Link}
        to="/cities"
      >
        Cities
      </MenuItem>
      <Divider />
      <MenuItem
        onClick={(e) => {handleToggleDrawer(e); handleCloseMenu()}}
        component={Link}
        to="/companies"
      >
        Companies / Institution
      </MenuItem>
    </Fragment>
  );
}

export function MenuSystem({color}: MenuSystemProps) {
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
        <Box>System</Box>
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
        <SystemMenuList closeMenu={handleClose}/>
      </Menu>
    </div>
  );
}

export function ResponsiveMenuSystem({closeToggleDrawer}: ResponsiveMenuProps) {
  const [open, setOpen] = useState(false);

  const handleToggleDrawer = (e: React.KeyboardEvent | React.MouseEvent) => {
    if(closeToggleDrawer)
      closeToggleDrawer(e);
  };

  return (
    <div>
      <ListItemButton onClick={() => { setOpen(!open); }}>
        <ListItemText primary="System" />
        {open ? <ExpandLess /> : <ExpandMore />}
      </ListItemButton>
      <Collapse
        in={open}
        timeout="auto"
        unmountOnExit
        sx={{ backgroundColor: "rgba(0, 0, 0, 0.1)" }}
      >
        <List component="div" disablePadding >
          <SystemMenuList closeToggleDrawer={handleToggleDrawer}/>
        </List>
      </Collapse>
    </div>
  );
}