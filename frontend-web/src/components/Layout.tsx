import Box, {BoxProps} from "@mui/joy/Box";
import Grid, {GridProps} from "@mui/joy/Grid";
import React from "react";

const Root = (props: BoxProps) => (
    <Box
        {...props}
        sx={[
            {
                bgcolor: "background.body",
                minHeight: "100vh",
            },
            ...(Array.isArray(props.sx) ? props.sx : [props.sx]),
        ]}
    />
);

const OnePage = (props: GridProps) => (
    <Grid
        container
        spacing={0}
        alignItems="center"
        justifyContent="center"
        mx={{minHeight: "100vh"}}
    >
        <Grid 
            md={3}
            justifyContent="center" {...props}
            sx={[
                {
                    bgcolor: "background.body",
                    minHeight: "100vh",
                },
                ...(Array.isArray(props.sx) ? props.sx : [props.sx]),
            ]} />
    </Grid>
);

const Header = (props: BoxProps) => (
    <Box
        component="header"
        className="Header"
        {...props}
        sx={[
            {
                p: 2,
                gap: 2,
                bgcolor: "background.componentBg",
                display: "flex",
                flexDirection: "row",
                justifyContent: "space-between",
                alignItems: "center",
                gridColumn: "1 / -1",
                borderBottom: "1px solid",
                borderColor: "divider",
                position: "sticky",
                top: 0,
                zIndex: 1100,
            },
            ...(Array.isArray(props.sx) ? props.sx : [props.sx]),
        ]}
    />
);

const SideNav = (props: BoxProps) => (
    <Box
        component="nav"
        className="Navigation"
        {...props}
        sx={[
            {
                p: 2,
                bgcolor: "background.componentBg",
                borderRight: "1px solid",
                borderColor: "divider",
            },
            ...(Array.isArray(props.sx) ? props.sx : [props.sx]),
        ]}
    />
);

const Main = (props: BoxProps) => (
    <Box
        component="main"
        className="Main"
        {...props}
        sx={[{p: 2}, ...(Array.isArray(props.sx) ? props.sx : [props.sx])]}
    />
);

const Layout = {
    Root,
    OnePage,
    Header,
    SideNav,
    Main,
};

export default Layout;
