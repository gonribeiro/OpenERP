import { useState } from 'react';
import { useAuth } from '../../contexts/AuthContext';

import { MenuEmployee, ResponsiveMenuEmployee } from './MenuEmployee';
import { MenuSystem, ResponsiveMenuSystem } from './MenuSystem';

import LogoOpenERP from '../../assets/LogoOpenErp';

import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Menu from '@mui/material/Menu';
import MenuIcon from '@mui/icons-material/Menu';
import Container from '@mui/material/Container';
import { AccountCircle } from '@mui/icons-material';
import Drawer from '@mui/material/Drawer';
import List from '@mui/material/List';
import MenuItem from '@mui/material/MenuItem';
import Brightness4Icon from '@mui/icons-material/Brightness4';
import Brightness7Icon from '@mui/icons-material/Brightness7';
import { Link } from 'react-router-dom';
import { Button, ListItemButton, ListItemText } from '@mui/material';

interface CustomNavbarProps {
  handleTheme: (close: boolean) => void;
  openApiThemeMode: "light" | "dark";
}

function CustomNavbar({ handleTheme, openApiThemeMode }: CustomNavbarProps) {
  const { logout } = useAuth();
  const [openUserMenu, setOpenUserMenu] = useState<null | HTMLElement>(null);
  const [openToggleDrawer, setOpenToggleDrawer] = useState(false);

  const handleChangeTheme = () => {
    handleTheme(false);
  };

  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setOpenUserMenu(event.currentTarget);
  };

  const handleToggleDrawer =
    (event: React.KeyboardEvent | React.MouseEvent) => {
      if (event.type === 'keydown'
        && ((event as React.KeyboardEvent).key === 'Tab'
        || (event as React.KeyboardEvent).key === 'Shift')
      ) {
        return;
      }

      setOpenToggleDrawer(!openToggleDrawer);
  };

  const list = () => (
    <Box
      role="presentation"
      onKeyDown={handleToggleDrawer}
    >
      <List
        sx={{ width: '100%', maxWidth: 360, bgcolor: 'background.paper' }}
        component="nav"
        aria-labelledby="nested-list-subheader"
      >
        <ListItemButton
          component={Link}
          to="/dashboard"
          onClick={handleToggleDrawer}
        >
          <ListItemText primary="Dashboard" />
        </ListItemButton>
        <ResponsiveMenuEmployee closeToggleDrawer={handleToggleDrawer} />
        <ResponsiveMenuSystem closeToggleDrawer={handleToggleDrawer} />
      </List>
    </Box>
  );

  return (
    <Container sx={{ paddingBottom: '80px' }}>
      <AppBar position="fixed" sx={{ width: '100%' }}>
        <Container maxWidth="xl">
          <Toolbar disableGutters>
            <LogoOpenERP />

            {/* Responsive Menu */}
            <Box sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}>
              <IconButton
                size="large"
                aria-label="account of current user"
                aria-controls="menu-appbar"
                aria-haspopup="true"
                onClick={handleToggleDrawer}
                color="inherit"
              >
                <MenuIcon />
              </IconButton>
            </Box>
            <Drawer
              anchor='left'
              open={openToggleDrawer}
              onClose={handleToggleDrawer}
            >
              {list()}
            </Drawer>

            {/* Menu */}
            <LogoOpenERP xs={'flex'} md={'none'} flexGrow={1} />
            <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
              <Button
                sx={{ color: 'white', display: 'block' }}
                component={Link}
                to="/dashboard"
              >
                Dashboard
              </Button>
              <MenuEmployee />
              <MenuSystem />
            </Box>

            {/* UserMenu */}
            <Box sx={{ flexGrow: 0, display: { xs: 'flex', md: 'flex' } }}>
              <IconButton onClick={handleChangeTheme}>
                {
                  openApiThemeMode === 'light'
                  ? <Brightness4Icon sx={{color: 'white' }}/>
                  : <Brightness7Icon sx={{color: 'white' }}/>
                }
              </IconButton>
              <IconButton onClick={handleOpenUserMenu}>
                <AccountCircle sx={{color: 'white' }}/>
              </IconButton>
              <Menu
                sx={{ mt: '40px' }}
                id="menu-appbar"
                anchorEl={openUserMenu}
                anchorOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                keepMounted
                transformOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                open={Boolean(openUserMenu)}
                onClose={() => {setOpenUserMenu(null)}}
              >
                <MenuItem onClick={() => {setOpenUserMenu(null)}}>
                  <Typography onClick={logout}>Logout</Typography>
                </MenuItem>
              </Menu>
            </Box>
          </Toolbar>
        </Container>
      </AppBar>
    </Container>
  );
}

export default CustomNavbar;