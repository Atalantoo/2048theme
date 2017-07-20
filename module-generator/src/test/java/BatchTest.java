import static java.lang.String.format;

import org.junit.Test;

public class BatchTest {

	static String path = System.getProperty("user.dir");
	static String inpTpl = path + "\\..\\resources\\%s.csv";
	static String outTpl = path + "\\..\\app-%s\\Assets\\Resources\\Sprites";

	@Test
	public void all() {
		String[] args = new String[] { //
				"cars"//
		};
		for (String arg : args) {
			Batch.main(new String[] { //
					"--src=" + format(inpTpl, arg), //
					"--dest=" + format(outTpl, arg) });
		}

	}

	@Test
	public void one_job() {
		String arg = "cars";
		Batch.main(new String[] { //
				"--src=" + format(inpTpl, arg), //
				"--dest=" + format(outTpl, arg) });
	}
}
