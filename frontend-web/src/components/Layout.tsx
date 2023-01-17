import MonetizationOnIcon from "@mui/icons-material/MonetizationOn";
import Box, {BoxProps} from "@mui/joy/Box";
import {GridProps} from "@mui/joy/Grid";
import IconButton from "@mui/joy/IconButton";
import List from "@mui/joy/List";
import Typography from "@mui/joy/Typography";
import React from "react";
import {Trans, useTranslation} from "react-i18next";

import {TranslationNamespaces} from "../locales/namespaces";
import {menu} from "../menu";
import {LogoutButton} from "./LogoutButton";
import AppMenuItem from "./MenuItem";

const RootCenter = (props: BoxProps) => (
    <Box
        display="flex"
        alignItems="center"
        justifyContent="center"
        sx={{
            bgcolor: "background.body",
            minHeight: "100vh",
        }}
    >
        <Box
            {...props}
            sx={[
                {
                    width: "auto",
                    bgcolor: "background.surface",
                },
                ...(Array.isArray(props.sx) ? props.sx : [props.sx]),
            ]}
        />
    </Box>
);

const RootDefault = (props: GridProps) => (
    <Box
        {...props}
        sx={[
            {
                bgcolor: "background.body",
                minHeight: "100vh",
                display: "flex",
                flexDirection: "column"
            },
            ...(Array.isArray(props.sx) ? props.sx : [props.sx]),
        ]}
    />
);

const Header = (props: GridProps) => {
    const {t} = useTranslation(TranslationNamespaces.Core);
    
    return (
        <Box
            component="header"
            alignItems="center"
            {...props}
            sx={[
                {
                    padding: 2,
                    gap: 2,
                    bgcolor: "background.surface",
                    borderBottom: "1px solid",
                    borderColor: "divider",
                    display: "flex",
                },
                ...(Array.isArray(props.sx) ? props.sx : [props.sx]),
            ]}
        >
            <IconButton
                size="sm"
                variant="solid"
                sx={{display: {sm: "inline-flex"}}}
            >
                <MonetizationOnIcon/>
            </IconButton>
            <Typography component="h1" fontWeight="xl" sx={{flexGrow: 1}}>
                {t("appTitle")}
            </Typography>

            <LogoutButton/>
        </Box>
    );
}

const SideNav = (props: BoxProps) => (
    <Box
        component="nav"
        {...props}
        sx={[
            {
                padding: 0,
                bgcolor: "background.surface",
                borderRight: "1px solid",
                borderColor: "divider",
            },
            ...(Array.isArray(props.sx) ? props.sx : [props.sx]),
        ]}
    >
        <List sx={{padding: 0}}>
            {menu.map(item => <AppMenuItem key={item.path} item={item}/>)}
        </List>
    </Box>
);

const MainWrapper = (props: BoxProps) => (
    <Box
        justifyContent="flex-start"
        sx={[
            {
                flexGrow: "1",
                display: "flex",
                flexDirection: "row-reverse",
            },
            ...(Array.isArray(props.sx) ? props.sx : [props.sx]),
        ]}
        {...props}
    />
);

const Main = (props: BoxProps) => (
    <Box
        sx={[
            {
                flexGrow: "1",
                paddingLeft: 2,
                paddingRight: 2,
                paddingTop: 2
            },
            ...(Array.isArray(props.sx) ? props.sx : [props.sx]),
        ]}
        {...props}
    />
);

const Footer = (props: BoxProps) => (
    <Box
        component="footer"
        {...props}
        sx={[
            {
                padding: 2,
                bgcolor: "background.surface",
                borderTop: "1px solid",
                borderColor: "divider",
            },
            ...(Array.isArray(props.sx) ? props.sx : [props.sx]),
        ]}
    >
        <small>
            <Trans i18nKey="runMode" values={{mode: process.env.NODE_ENV}} components={{bold: <strong />}} />
        </small>
    </Box>
);

const Layout = {
    RootCenter,
    RootDefault,
    Header,
    SideNav,
    MainWrapper,
    Main,
    Footer,
};

export default Layout;
