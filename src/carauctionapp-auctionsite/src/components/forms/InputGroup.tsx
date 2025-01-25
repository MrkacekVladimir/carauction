import classes from "./InputGroup.module.css";
interface Props {
  children: React.ReactNode;
}

export const InputGroup = ({ children }: Props) => {
  return <div className={classes.inputGroup}>{children}</div>;
};
