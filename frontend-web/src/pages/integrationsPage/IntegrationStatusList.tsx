import List from "@mui/joy/List";

interface IntegrationStatusListProps {
    children: JSX.Element;
}

const IntegrationStatusList = (props: IntegrationStatusListProps) => (
    <List  sx={{"--List-decorator-size": "48px"}}>
        {props.children}
    </List>
)

export default IntegrationStatusList;
