import {extendTheme} from "@mui/joy/styles";

const theme = extendTheme({
    colorSchemes: {
        light: {
            palette: {
                background: {
                    body: "var(--joy-palette-neutral-50)",
                    surface: "var(--joy-palette-common-white)",
                },
            },
        },
        dark: {
            palette: {
                background: {
                    body: "var(--joy-palette-common-black)",
                    surface: "var(--joy-palette-neutral-900)",
                },
            },
        },
    },
    fontFamily: {
        // display: "'Inter', var(--joy-fontFamily-fallback)",s
        // body: "'Inter', var(--joy-fontFamily-fallback)",
    },
});

export default theme;