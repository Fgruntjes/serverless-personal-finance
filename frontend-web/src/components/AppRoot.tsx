import BookRoundedIcon from '@mui/icons-material/BookRounded';
import MonetizationOnIcon from '@mui/icons-material/MonetizationOn';
import {Box, IconButton, Typography} from "@mui/joy";
import React, {Suspense} from 'react';
import {useTranslation} from "react-i18next";
import {Outlet} from "react-router-dom";

import Layout from "./Layout"

function AppRoot() {
    const {t} = useTranslation();
    
    return (
        <Suspense fallback="Loading...">
            <Layout.Root>
                <Layout.Header>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            alignItems: 'center',
                            gap: 1.5,
                        }}
                    >
                        <IconButton
                            size="sm"
                            variant="solid"
                            sx={{display: {sm: 'inline-flex'}}}
                        >
                            <MonetizationOnIcon />
                        </IconButton>
                        <Typography component="h1" fontWeight="xl">
                            {t('appTitle')}
                        </Typography>
                    </Box>
                    
                    <Box sx={{display: 'flex', flexDirection: 'row', gap: 1.5}}>
                        <BookRoundedIcon />
                    </Box>
                </Layout.Header>
                <Layout.Main>
                    <Outlet />
                </Layout.Main>
                <small>You are running this application in <b>{process.env.NODE_ENV}</b> mode.</small>
            </Layout.Root>
        </Suspense>
    );
}

export default AppRoot;
