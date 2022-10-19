import {Typography, TypographyProps} from "@mui/joy";

const PageTitle = (props: TypographyProps) => {
    return (
        <Typography
            component="h1"
            {...props}
            sx={{
                backgroundColor: "background.surface",
                paddingLeft: 2,
                paddingRight: 2,
                paddingBottom: 1,
                paddingTop: 1,
                borderBottom: 1,
                borderColor: "divider",
            }} />
    );
}

export default PageTitle;