import SyncAltIcon from "@mui/icons-material/SyncAlt";
import React from "react";

import {paths} from "./paths";

export type MenuItem = {
    label: string,
    path: string,
    icon: JSX.Element,
}

export const menu: MenuItem[] = [
    {
        label: "menu.integrations",
        path: paths.integrations,
        icon: <SyncAltIcon />,
    }
]