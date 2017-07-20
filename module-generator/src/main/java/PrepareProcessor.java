import static com.google.common.base.Preconditions.checkArgument;
import static org.apache.commons.lang3.StringUtils.isNotBlank;

import java.io.File;
import java.util.ArrayList;

import org.apache.commons.io.FilenameUtils;
import org.springframework.batch.item.ItemProcessor;
import org.springframework.core.io.Resource;

import lombok.AllArgsConstructor;
import lombok.NonNull;

@AllArgsConstructor
public class PrepareProcessor implements ItemProcessor<Prepare, Prepare> {

	@NonNull
	private Resource src;
	@NonNull
	private Resource des;

	@Override
	public Prepare process(Prepare item) throws Exception {
		checkArgument(isNotBlank(item.getName()));
		
		File parent = src.getFile().getParentFile();
		String fileNameWithOutExt = FilenameUtils.removeExtension(src.getFilename());
		
		item.setPictures(new ArrayList<>());
		item.getPictures()
				.add(new Picture( //
						parent.toPath().toString() + "/" + fileNameWithOutExt + "/0000.bmp", //
						des.getFile().toPath().toString() + "/0000.png"));
		return item;
	}

}
