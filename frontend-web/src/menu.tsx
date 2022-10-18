import SyncIcon from "@mui/icons-material/Sync";
import React from "react";

import {paths} from "./paths";

export type MenuItem = {
    label: string,
    path: string,
    icon: JSX.Element,
}

export const menu: MenuItem[] = [
    {
        label: "menu.imports",
        path: paths.imports,
        icon: <SyncIcon />,
    }
]