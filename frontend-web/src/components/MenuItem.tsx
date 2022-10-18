import ListItem from "@mui/joy/ListItem";
import ListItemButton from "@mui/joy/ListItemButton";
import ListItemDecorator from "@mui/joy/ListItemDecorator";
import React from "react";
import {useTranslation} from "react-i18next";
import {useNavigate} from "react-router-dom";

import {MenuItem} from "../menu";

type AppMenuItemProps = {
    item: MenuItem,
    key: string|null|undefined,
}

const AppMenuItem = (props: AppMenuItemProps) => {
    const {
        item: {
            path, icon, label
        }
    } = props;
    const navigate = useNavigate();
    const {t} = useTranslation();
    
    return (
        <ListItem key={props.key}>
            <ListItemButton onClick={() => navigate(path)}>
                <ListItemDecorator>
                    {icon}
                </ListItemDecorator>
                {t(label)}
            </ListItemButton>
        </ListItem>
    );
}

export default AppMenuItem;