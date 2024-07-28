import { Button, ButtonProps, CircularProgress } from "@mui/material";

interface SaveButtonProps extends ButtonProps {
  loading: boolean;
  color?: "inherit" | "primary" | "secondary" | "success" | "error" | "info" | "warning";
  buttonName?: string;
  variant?: "text" | "outlined" | "contained";
}

function SaveButton({ loading, color, buttonName, variant }: SaveButtonProps) {
  return (
    <Button
      type="submit"
      variant={variant ?? "contained"}
      color={color ?? "primary"}
    >
      { loading
        ? <CircularProgress style={{ color: "white" }} size={25} />
        : buttonName ?? "Save"
      }
    </Button>
  );
}

export default SaveButton;