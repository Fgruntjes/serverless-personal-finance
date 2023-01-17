import Box from "@mui/joy/Box";
import ListDivider from "@mui/joy/ListDivider";
import ListItem from "@mui/joy/ListItem";
import ListItemContent from "@mui/joy/ListItemContent";
import ListItemDecorator from "@mui/joy/ListItemDecorator";
import {styled} from "@mui/system";
import React from "react";
import {useTranslation} from "react-i18next";

import {TranslationNamespaces} from "../../locales/namespaces";


interface IntegrationStatusListItemProps {
    name: string;
    icon: string;
    children: JSX.Element|JSX.Element[];
}

const FullWidthImage = styled("img")({
    width: "100%",
    height: "100%",
});

const StatusContainer = (props: IntegrationStatusListItemProps) => {
    const {t} = useTranslation(TranslationNamespaces.IntegrationsPage);
    
    return (
        <>
            <ListItem>
                <ListItemDecorator>
                    <FullWidthImage src={props.icon} alt={t(`name.${props.name}`)} />
                </ListItemDecorator>
                <ListItemContent>
                    <Box sx={{
                        display: "flex", gap: 2, width: "100%", flexDirection: "column" 
                    }}>
                        {props.children}
                    </Box>
                </ListItemContent>
            </ListItem>
            <ListDivider />
        </>
    );
}

export default StatusContainer;
