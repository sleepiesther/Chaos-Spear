namespace Chaos_Spear {
	public class HookAlreadyCreatedException : Exception {
		public HookAlreadyCreatedException() {
		}

		public HookAlreadyCreatedException(string? message) : base(message) {
		}

		public HookAlreadyCreatedException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}

	public class HookSizeMismatchException : Exception {
		public HookSizeMismatchException() {
		}

		public HookSizeMismatchException(string? message) : base(message) {
		}

		public HookSizeMismatchException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}

	public class SigNotFoundException : Exception {
		public SigNotFoundException() {
		}

		public SigNotFoundException(string? message) : base(message) {
		}

		public SigNotFoundException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}

	public class InvalidAddressException : Exception {
		public InvalidAddressException() {
		}

		public InvalidAddressException(string? message) : base(message) {
		}

		public InvalidAddressException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}

	public class GameExitedException : Exception {
		public GameExitedException() {
		}

		public GameExitedException(string? message) : base(message) {
		}

		public GameExitedException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}
