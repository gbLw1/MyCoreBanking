export default interface ServiceResult<T = null> {
  success: boolean;
  data?: T | null;
  hasErrors: boolean;
  hasImpediments: boolean;
  hasWarnings: boolean;
  messages: ServiceMessage[];
}

interface ServiceMessage {
  messageType: "Error" | "Impediment" | "Warning";
  message: string;
  systemKey: string;
}
