import WarningIcon from "@mui/icons-material/Warning";
import Alert from "@mui/joy/Alert";
import Button from "@mui/joy/Button";
import Typography from "@mui/joy/Typography";
import React from "react";
import {ErrorBoundary, FallbackProps} from "react-error-boundary";
import {useTranslation} from "react-i18next";
import {useQueryErrorResetBoundary} from "react-query";

type AppFallbackProps = FallbackProps & {
    title: string;
}

function ErrorFallback({
    error, resetErrorBoundary, title
}: AppFallbackProps) {
    const {t} = useTranslation("error");
    
    return (
        <Alert
            color="danger"
            variant="outlined"
            sx={{alignItems: "flex-start"}}
            startDecorator={<WarningIcon sx={{mt: "2px", mx: "4px"}} fontSize="large"/>}
        >
            <div>
                <Typography fontWeight="lg" mt={0.25}>
                    {title}
                </Typography>
                <Typography fontSize="sm" sx={{opacity: 0.8}}>
                    {error.message}
                </Typography>
                <Button onClick={resetErrorBoundary}>{t("button.retry")}</Button>
            </div>
        </Alert>
    );
}

type ComponentWithErrorProps = {
    errorTitle?: string;
}

export default function withComponentErrorBoundary<T extends ComponentWithErrorProps>(
    WrappedComponent: React.ComponentType<T>
) {
    const displayName = WrappedComponent.displayName || WrappedComponent.name || "Component";

    const ComponentWithErrorBoundary = (props: T) => {
        const {reset} = useQueryErrorResetBoundary()
        const {t} = useTranslation("error");
        const errorTitle = props.errorTitle ? props.errorTitle : t("component.label");

        return (
            <ErrorBoundary
                onReset={reset}
                fallbackRender={({error, resetErrorBoundary}) => 
                    <ErrorFallback
                        error={error}
                        resetErrorBoundary={resetErrorBoundary}
                        title={errorTitle}/>
                }
            >
                <WrappedComponent {...props} />
            </ErrorBoundary>
        );
    };

    ComponentWithErrorBoundary.displayName = `${withComponentErrorBoundary.name}(${displayName})`;

    return ComponentWithErrorBoundary;
}
